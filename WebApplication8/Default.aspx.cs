using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace WebApplication8
{
    public partial class Default : System.Web.UI.Page
    {

        private static QueueClient queueClient;



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Button4_Click(sender,e);
                //updateListbox();

            }
        }

      

        protected void Button1_Click(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(TextBox2.Text,out value))
            {
                SendMessages(value);
                Button4_Click(sender,e);
            }
        }

        private static void SendMessages(int count)
        {



            List<BrokeredMessage> smalljobs = new List<BrokeredMessage>();
            List<BrokeredMessage> bigjobs = new List<BrokeredMessage>();
            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                var g = Guid.NewGuid();

                int x = random.Next(300);



                InsertDB(g,x);

                if (x > 240)
                  bigjobs.Add(CreateSampleMessage(g, "Message number: " + i));
                else
                  smalljobs.Add(CreateSampleMessage(g, "Message number: " + i));

            }
            SendMessages("bus1",smalljobs);
            SendMessages("bus2",bigjobs);
        }

        private static void InsertDB(Guid g, int jobsize)
        {
            string bus;
            if (jobsize > 240)
            {
                bus = "bus2";
            }
            else
            {
                bus = "bus1";
                jobsize = jobsize/5;
            }



            SqlDataSource s = new SqlDataSource();
            s.ConnectionString = ConfigurationManager.ConnectionStrings["App1DBConnectionString"].ToString();
            s.InsertCommand = @"Insert into [dbo].[TEST] (Id,Count,Remarks,JobSize,UserId,DTReceived) Values (@g,1,@remarks,@jobsize,@UserId,@DTnow)";

            s.InsertParameters.Add("g", g.ToString());
            s.InsertParameters.Add("remarks", bus);
            s.InsertParameters.Add("jobsize", jobsize.ToString());
            s.InsertParameters.Add("UserId", Guid.NewGuid().ToString());
            s.InsertParameters.Add("DTNow", DbType.DateTime, DateTime.UtcNow.ToString());

            s.Insert();
        }
        private static void SendMessages(string queuename, List<BrokeredMessage> smalljobs)
        {
            queueClient = QueueClient.Create(queuename);
            foreach (BrokeredMessage message in smalljobs)
            {
                while (true)
                {
                    try
                    {
                        queueClient.Send(message);
                    }
                    catch (MessagingException e)
                    {
                        if (!e.IsTransient)
                        {
                            throw;
                        }
                        else
                        {
                            HandleTransientErrors(e);
                        }
                    }
                    break;
                }
            }
        }

       

        public static List<string> ReceiveMessages()
        {
            
            List<string> l = new List<string>();
            
            queueClient = QueueClient.Create("Bus1");
          

            //var messages = queueClient.ReceiveBatch(10);
            //foreach (var message in messages)
            //{
            //  l.Add(string.Format("Message received: Id = {0}, Body = {1}", message.MessageId, message.GetBody<string>()));
            //}
               
            //queueClient.Close();

            return l;
        }


        private static void HandleTransientErrors(MessagingException e)
        {
            //If transient error/exception, let’s back-off for 2 seconds and retry 
            Console.WriteLine(e.Message);
            Console.WriteLine("Will retry sending the message in 2 seconds");
            Thread.Sleep(2000);
        }
        private static BrokeredMessage CreateSampleMessage(Guid messageId, string messageBody)
        {
            BrokeredMessage message = new BrokeredMessage(messageBody);
            message.MessageId = messageId.ToString();
            return message;
        }

       

       
       

        protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

       

       

        protected void Button5_Click(object sender, EventArgs e)
        {
            SendMessages(10);
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            GridView1.DataBind();
            queueClient = QueueClient.Create("bus2");



            NamespaceManager nsmgr = Microsoft.ServiceBus.NamespaceManager.CreateFromConnectionString(System.Configuration.ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"]);
            long count1 = nsmgr.GetQueue("bus1").MessageCount;
            long count2 = nsmgr.GetQueue("bus2").MessageCount;

            Label1.Text = count1.ToString();
            Label2.Text = count2.ToString();

            var connectionString = ConfigurationManager.ConnectionStrings["App1DBConnectionString"].ToString();
            SqlConnection sqlConnection1 = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection1;
            sqlConnection1.Open();
            cmd.CommandText = "Select Count('Id') From [dbo].[TEST] where [DTProcessStarted] is not null and [DTProcessEnded] is null";
            int tt = (int) cmd.ExecuteScalar();
            Label3.Text = tt.ToString();
        }

        protected void Button5_Click1(object sender, EventArgs e)
        {

        }
    }
}