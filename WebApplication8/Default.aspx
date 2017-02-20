<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication8.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style2 {
            width: 566px;
        }
        .auto-style3 {
            width: 1211px;
        }
        .auto-style4 {
            width: 566px;
            height: 85px;
        }
        .auto-style5 {
            width: 1211px;
            height: 85px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table style="width:100%;">
            <tr>
                <td class="auto-style4">
                    <asp:TextBox ID="TextBox2" runat="server" Width="42px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Add jobs" Width="74px" />
                </td>
                <td class="auto-style5">
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td class="auto-style2">
                    Bus1 messages: <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="X-Large" ForeColor="#3399FF" Text="0"></asp:Label>
                    <br />
                    Bus2 messages:
                    <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="X-Large" ForeColor="#0099FF" Text="0"></asp:Label>
                </td>
                <td class="auto-style3">Jobs being processed:
                    <asp:Label ID="Label3" runat="server" Font-Names="Arial" Font-Size="X-Large" ForeColor="#0099FF" Text="0"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">
                    <br />
&nbsp;<asp:Button ID="Button4" runat="server" OnClick="Button4_Click" Text="Refresh" />
                    </td>
                <td class="auto-style3">
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:App1DBConnectionString %>" SelectCommand="SELECT * FROM [TEST] WHERE ([DTProcessEnded] IS NULL) ORDER BY [DTProcessStarted] DESC, [Remarks] DESC"></asp:SqlDataSource>
                    <br />
        <br />
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None" Width="1117px">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                            <asp:BoundField DataField="Count" HeaderText="Count" SortExpression="Count" />
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
                            <asp:BoundField DataField="JobSize" HeaderText="JobSize" SortExpression="JobSize" />
                            <asp:BoundField DataField="UserId" HeaderText="UserId" SortExpression="UserId" />
                            <asp:BoundField DataField="DTReceived" HeaderText="DTReceived" SortExpression="DTReceived" />
                            <asp:BoundField DataField="DTProcessStarted" HeaderText="DTProcessStarted" SortExpression="DTProcessStarted" />
                            <asp:BoundField DataField="DTProcessEnded" HeaderText="DTProcessEnded" SortExpression="DTProcessEnded" />
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
        <br />
        <br />
        <br />
        <br />
        <p>
                    &nbsp;</p>
    </form>
</body>
</html>
