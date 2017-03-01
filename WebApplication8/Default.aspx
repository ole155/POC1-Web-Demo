<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication8.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style2 {
            width: 1486px;
        }
        .auto-style3 {
            width: 1211px;
        }
        .auto-style4 {
            width: 1486px;
            height: 85px;
        }
        .auto-style5 {
            width: 1211px;
            height: 85px;
        }
        .auto-style6 {
            width: 1486px;
            height: 36px;
        }
        .auto-style7 {
            width: 1211px;
            height: 36px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table style="width:100%;">
            <tr>
                <td class="auto-style6">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="Label4" runat="server" Font-Names="Arial" Font-Size="XX-Large" ForeColor="#507CD1"></asp:Label>
                            <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
                            </asp:Timer>
                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="auto-style7">
                </td>
                <td class="auto-style7">
                </td>
            </tr>
            <tr>
                <td class="auto-style4">
                    Queue jobs:<br />
                    <br />
                    <asp:TextBox ID="TextBox2" runat="server" Width="42px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Add jobs to Queue" Width="155px" />
                </td>
                <td class="auto-style5">
                    Live jobs:<br />
                    <br />
                    <asp:TextBox ID="TextBox3" runat="server" Width="304px"></asp:TextBox>
&nbsp;<asp:Button ID="Button7" runat="server" OnClick="Button7_Click" Text="Execute new job Synchronously" />
&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Button12" runat="server" OnClick="Button12_Click" Text="Execute 50" />
                    <br />
                </td>
                <td class="auto-style5">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style2">
                    Bus1 messages: <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="X-Large" ForeColor="#507CD1" Text="0" Width="50px"></asp:Label>
                    Dead Letters:
                    <asp:Label ID="Label7" runat="server" Font-Names="Arial" Font-Size="X-Large" ForeColor="#507CD1" Text="0" Width="50px"></asp:Label>
                &nbsp;&nbsp;
                    <asp:Button ID="Button9" runat="server" OnClick="Button9_Click" Text="Retry " Width="79px" />
                    <br />
                    Bus2 messages:
                    <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="X-Large" ForeColor="#507CD1" Text="0" Width="50px"></asp:Label>
                    Dead Letters:
                    <asp:Label ID="Label8" runat="server" Font-Names="Arial" Font-Size="X-Large" ForeColor="#507CD1" Text="0" Width="50px"></asp:Label>
                &nbsp;&nbsp;
                    <asp:Button ID="Button11" runat="server" OnClick="Button11_Click" Text="Retry " Width="79px" />
                </td>
                <td class="auto-style3">Jobs being processed:
                    <asp:Label ID="Label3" runat="server" Font-Names="Arial" Font-Size="X-Large" ForeColor="#507CD1" Text="0"></asp:Label>
                </td>
                <td class="auto-style3">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style2">
                    <br />
&nbsp;<asp:Button ID="Button4" runat="server" OnClick="Button4_Click" Text="Refresh" />
                    &nbsp;
                    <asp:Button ID="Button8" runat="server" OnClick="Button8_Click" Text="Get UTC" />
                    &nbsp;
                            <asp:Label ID="Label5" runat="server" Font-Names="Arial" Font-Size="Large" ForeColor="#507CD1"></asp:Label>
                    </td>
                <td class="auto-style3">
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button10" runat="server" OnClick="Button10_Click" Text="Button" />
                            </td>
                <td class="auto-style3">
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
                    <br />
        <br />
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None" Width="1117px" AutoGenerateSelectButton="True" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowCommand="GridView1_RowCommand">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                            <asp:BoundField DataField="Renewals" HeaderText="Renewals" SortExpression="Renewals" />
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
                            <asp:BoundField DataField="JobSize" HeaderText="JobSize" SortExpression="JobSize" />
                            <asp:BoundField DataField="UserId" HeaderText="UserId" SortExpression="UserId" />
                            <asp:BoundField DataField="DTReceived" HeaderText="DTReceived" SortExpression="DTReceived" />
                            <asp:BoundField DataField="DTProcessStarted" HeaderText="DTProcessStarted" SortExpression="DTProcessStarted" />
                            <asp:BoundField DataField="DTProcessEnded" HeaderText="DTProcessEnded" SortExpression="DTProcessEnded" />
                            <asp:BoundField DataField="TotalProcessTime" HeaderText="TotalProcessTime (Netto)" SortExpression="TotalProcessTime" />
                            <asp:ButtonField ButtonType="Button" Text="ReRun" />
                        </Columns>
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
        <br />
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:App1DBConnectionString %>" SelectCommand="SELECT TOP (500) Id, Renewals, Remarks, JobSize, UserId, DTReceived, DTProcessStarted, DTProcessEnded, TotalProcessTime FROM TEST ORDER BY DTReceived DESC "></asp:SqlDataSource>
        <br />
        <br />
        <br />
        <br />
        <p>
                    &nbsp;</p>
    </form>
</body>
</html>
