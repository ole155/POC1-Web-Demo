using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
using RestSharp;

namespace WebApplication8
{
    public partial class Default : System.Web.UI.Page
    {

        private static QueueClient queueClient;
        private static RestClient serverClient = new RestClient("http://ohmapp2.azurewebsites.net/api/Default/");
        // private static RestClient serverClient = new RestClient("http://localhost:50363/api/Default/");



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Refresh();
                //updateListbox();

            }
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(TextBox2.Text, out value))
            {
                AddJobsAsync(value);
                Refresh();
            }
        }

        Random random = new Random();

        private void AddJobsAsync(int count)
        {



            List<BrokeredMessage> smalljobs = new List<BrokeredMessage>();
            List<BrokeredMessage> bigjobs = new List<BrokeredMessage>();

            for (int i = 0; i < count; i++)
            {
                var g = Guid.NewGuid();

                int x = random.Next(300);

                if (x <= 290)
                {
                    if (InsertDB(g, 1, "bus1"))
                    {
                        TopicDescription td = new TopicDescription("Hej");
                        td.EnableExpress = true;
                        SendMessage("bus1", CreateMessage(g, "Message number: " + i));
                    }
                }
                else
                {
                    if (InsertDB(g, 1, "bus2"))
                    {
                        SendMessage("bus2", CreateMessage(g, "Message number: " + i));
                    }
                }



            }

        }

        private static bool InsertDB(Guid id, int jobsize, string bus)
        {

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["App1DBConnectionString"].ToString();
                SqlConnection sqlConnection1 = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlConnection1;
                sqlConnection1.Open();

                cmd.CommandText = @"Insert into [dbo].[TEST] (Id,Renewals,Remarks,JobSize,UserId,DTReceived) Values (@id,0,@remarks,@jobsize,@UserId,@DTnow)";
                cmd.Parameters.AddWithValue("id", id.ToString());
                cmd.Parameters.AddWithValue("remarks", bus);
                cmd.Parameters.AddWithValue("jobsize", jobsize.ToString());
                cmd.Parameters.AddWithValue("UserId", Guid.NewGuid().ToString());
                DateTime now = GetServerDateTime(serverClient);
                cmd.Parameters.AddWithValue("DTNow", now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
                var tt = cmd.ExecuteNonQuery();
                sqlConnection1.Close();
            }
            catch (Exception ex)
            {

                return false;
            }



            return true;

        }

        private static bool ItemExists(Guid id)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["App1DBConnectionString"].ToString();
            SqlConnection sqlConnection1 = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection1;
            sqlConnection1.Open();
            cmd.CommandText = "Select [Id] From [dbo].[TEST] where [Id] = @id";
            cmd.Parameters.AddWithValue("id", id.ToString());
            var tt = cmd.ExecuteReader().HasRows;
            sqlConnection1.Close();

            return tt;
        }

        private static void UpdateReceivedDB(string g)
        {

            SqlDataSource s = new SqlDataSource();
            s.ConnectionString = ConfigurationManager.ConnectionStrings["App1DBConnectionString"].ToString();
            s.UpdateCommand = @"Update [dbo].[TEST] SET [DTReceived] = @DTNow, [DTProcessStarted] = @reset, [DTProcessEnded] = @reset WHERE Id = @g";
            s.UpdateParameters.Add("g", g);
            s.UpdateParameters.Add("reset", DBNull.Value.ToString());
            DateTime now = GetServerDateTime(serverClient);
            s.UpdateParameters.Add("DTNow", DbType.DateTime, now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));

            s.Update();
        }



        private static void SendMessage(string queuename, BrokeredMessage message)
        {
            queueClient = QueueClient.Create(queuename);
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
        private static BrokeredMessage CreateMessage(Guid messageId, string messageBody)
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
            AddJobsAsync(10);
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
           Refresh();
        }

        protected void Button5_Click1(object sender, EventArgs e)
        {

        }

        protected void Button5_Click2(object sender, EventArgs e)
        {
            queueClient = QueueClient.Create("bus2",ReceiveMode.ReceiveAndDelete);
            var brokeredMessage = queueClient.Receive();
            Refresh();
            //while (queueClient.Peek() != null)
            //{
            //    var brokeredMessage = queueClient.Receive();
            //    if (brokeredMessage == null) return;
            //    brokeredMessage.Complete();

            //}

        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            AddJobSync(TextBox3.Text);
        }

        private void AddJobSync(string value)
        {
            Guid g;
            if (!Guid.TryParse(value, out g)) return;

            if (!ItemExists(g))
            {
                InsertDB(g, 0, "");
            }
            else
            {
                UpdateReceivedDB(value);
            }

            Label5.Text = "";

            var request = new RestRequest(Method.POST);
            request.AddParameter("", value, ParameterType.GetOrPost);
            IRestResponse response = serverClient.Execute(request);
            var content = response.Content; // raw content as string


            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                Thread.Sleep(100);
                Refresh();
            }
            else
            {
                Label5.Text = response.ErrorMessage;
            }


            GridView1.SelectedIndex = -1;
        }

        public void Refresh()
        {
            GridView1.DataBind();
            //            queueClient = QueueClient.Create("bus2");



            NamespaceManager nsmgr = Microsoft.ServiceBus.NamespaceManager.CreateFromConnectionString(System.Configuration.ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"]);

            var queue1details = nsmgr.GetQueue("bus1").MessageCountDetails;
            Label1.Text = queue1details.ActiveMessageCount.ToString();
            Label7.Text = queue1details.DeadLetterMessageCount.ToString();



            var queue2details = nsmgr.GetQueue("bus2").MessageCountDetails;
            Label2.Text = queue2details.ActiveMessageCount.ToString();
            Label8.Text = queue2details.DeadLetterMessageCount.ToString();


            var connectionString = ConfigurationManager.ConnectionStrings["App1DBConnectionString"].ToString();
            SqlConnection sqlConnection1 = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection1;
            sqlConnection1.Open();
            cmd.CommandText = "Select Count('Id') From [dbo].[TEST] where [DTProcessStarted] is not null and [DTProcessEnded] is null";
            int tt = (int)cmd.ExecuteScalar();
            Label3.Text = tt.ToString();
            sqlConnection1.Close();

     



        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            TextBox3.Text = Guid.NewGuid().ToString();
            AddJobSync(TextBox3.Text);
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox3.Text = GridView1.SelectedRow.Cells[1].Text;

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Label4.Text = DateTime.Now.ToLongTimeString();
        }

        protected void Button8_Click(object sender, EventArgs e)
        {
            var dt = GetServerDateTime(serverClient);

        }

        private static DateTime GetServerDateTime(RestClient reseClient)
        {
            var request = new RestRequest(Method.GET);

            IRestResponse response = reseClient.Execute(request);


            DateTime servertime = DateTime.Parse(response.Content.Split(',')[2].Trim(']').Trim('"'));

            return servertime;

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var g = GridView1.Rows[Convert.ToInt32(e.CommandArgument)].Cells[1].Text;
            AddJobSync(g);

        }

        protected void Button9_Click(object sender, EventArgs e)
        {
            NamespaceManager nsmgr = Microsoft.ServiceBus.NamespaceManager.CreateFromConnectionString(System.Configuration.ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"]);

            var queueDeadletterPath = QueueClient.FormatDeadLetterPath("Bus1");
            QueueClient deadletterQueueClient = QueueClient.Create(queueDeadletterPath);

            while (nsmgr.GetQueue("bus1").MessageCountDetails.DeadLetterMessageCount > 0)
            {
              
                var message = deadletterQueueClient.Receive();
                if (message != null)
                {
                    SendMessage("bus1",message.Clone());
                    message.Complete();
                }

            }


            Refresh();


        }


        protected void Button11_Click(object sender, EventArgs e)
        {
            NamespaceManager nsmgr = Microsoft.ServiceBus.NamespaceManager.CreateFromConnectionString(System.Configuration.ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"]);

            var queueDeadletterPath = QueueClient.FormatDeadLetterPath("Bus2");
            QueueClient deadletterQueueClient = QueueClient.Create(queueDeadletterPath);

            while (nsmgr.GetQueue("bus2").MessageCountDetails.DeadLetterMessageCount > 0)
            {

                var message = deadletterQueueClient.Receive();
                if (message != null)
                {
                    SendMessage("bus2", message.Clone());
                    message.Complete();
                }

            }


            Refresh();

        }

        protected void Button12_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 50; i++)
            {
                AddJobSync(Guid.NewGuid().ToString());
            }

        }
    }
}