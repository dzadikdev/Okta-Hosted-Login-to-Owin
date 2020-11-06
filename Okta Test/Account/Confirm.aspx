<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Confirm.aspx.cs" Inherits="Okta_Test.Account.Confirm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<p><br /><strong>You are in!!</strong><br /></p>
<table  class="greyGridTable">
  <tr>
    <th style="width:50%">ID Token</th>
    <th style="width:50%">Auth Token</th>
  </tr>
  <tr>
    <td style="width:50%"><asp:Label ID="idtokendecoded" runat="server" Text="Label"></asp:Label><br /></td>
    <td style="width:50%"><asp:Label ID="accesstokendecoded" runat="server" Text="Label"></asp:Label><br /></td>
  </tr>
</table>
<style>
table.greyGridTable {
  border: 2px solid 000000;
  width: 100%;
  border-collapse: collapse;
  word-break:break-all;
}
table.greyGridTable td, table.greyGridTable th {
  padding: 3px 4px;
  border: solid;
  border-color: darkgray;
}
table.greyGridTable td:nth-child(even) {
  background: #EBEBEB;
}
table.greyGridTable thead {
  background: #FFFFFF;
  border-bottom: 4px solid #333333;
}
table.greyGridTable thead th {
  font-size: 15px;
  font-weight: bold;
  color: #333333;
  text-align: center;
  border-left: 2px solid #333333;
}
table.greyGridTable thead th:first-child {
  border-left: none;
}

table.greyGridTable tfoot td {
  font-size: 14px;
}
</style>
</asp:Content>
