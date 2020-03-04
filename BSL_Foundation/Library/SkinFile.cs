
using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
  internal class SkinFile
  {
    private string _fullFileNamePath;
    private ApplicationVersion _appVersion;
    private ApplicationType _appType;
    private GridViewStyle _selectedGridViewStyle;

    internal SkinFile()
    {
    }

    internal SkinFile(string fullFileNamePath, ApplicationVersion appVersion, ApplicationType appType = ApplicationType.ASPNET, GridViewStyle selectedGridViewStyle = GridViewStyle.Professional)
    {
      this._fullFileNamePath = fullFileNamePath;
      this._appVersion = appVersion;
      this._appType = appType;
      if (this._appType == ApplicationType.ASPNET45)
        this._selectedGridViewStyle = selectedGridViewStyle;
      this.Generate();
    }

    private void Generate()
    {
      using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<asp:Textbox runat=\"Server\" Font-Size=\"12px\" Width=\"250px\" />");
        stringBuilder.AppendLine("<asp:Textbox SkinID=\"TextBoxDate\" runat=\"Server\" Font-Size=\"12px\" Width=\"234px\" />");
        stringBuilder.AppendLine("<asp:Textbox SkinID=\"TextBoxInline\" runat=\"Server\" Font-Size=\"12px\" Width=\"86px\" />");
        stringBuilder.AppendLine("<asp:Textbox SkinID=\"TextBoxDateInline\" runat=\"Server\" Font-Size=\"12px\" Width=\"86px\" />");
        stringBuilder.AppendLine("<asp:DropDownList runat=\"Server\" Font-Size=\"12px\" Width=\"262px\" />");
        stringBuilder.AppendLine("<asp:DropDownList SkinID=\"DropDownInline\" runat=\"Server\" Font-Size=\"12px\" Width=\"86px\" />");
        stringBuilder.AppendLine("<asp:Label runat=\"Server\" Font-Size=\"12px\" />");
        stringBuilder.AppendLine("<asp:Button runat=\"server\" Width=\"150px\" />");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("<asp:RequiredFieldValidator ForeColor=\"Red\" runat=\"server\" />");
        stringBuilder.AppendLine("<asp:CompareValidator ForeColor=\"Red\" runat=\"server\" />");
        if (this._appVersion != ApplicationVersion.Express)
        {
          string str1 = "4";
          string str2 = string.Empty;
          if (this._appType == ApplicationType.ASPNET45)
          {
            str1 = "8";
            str2 = " Width=\"100%\"";
          }
          if (this._appType == ApplicationType.ASPNET || this._appType == ApplicationType.ASPNET45 && this._selectedGridViewStyle == GridViewStyle.Colorful)
          {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewColorful\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"" + str2);
            stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
            stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" />");
            stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#990000\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#990000\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <PagerStyle BackColor=\"#FFCC66\" ForeColor=\"#333333\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
            stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
            stringBuilder.AppendLine("    <RowStyle BackColor=\"#FFFBD6\" ForeColor=\"#333333\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#FFCC66\" Font-Bold=\"True\" ForeColor=\"Navy\" />");
            stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FFFDEA\" />");
            stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#4D0000\" />");
            stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#FFFCDF\" />");
            stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#820000\" />");
            stringBuilder.AppendLine("</asp:GridView>");
            if (this._appType == ApplicationType.ASPNET45)
            {
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewColorfulNoPagingNoSorting\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"False\"" + str2);
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" />");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#990000\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#990000\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#FFCC66\" ForeColor=\"#333333\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#FFFBD6\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#FFCC66\" Font-Bold=\"True\" ForeColor=\"Navy\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FFFDEA\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#4D0000\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#FFFCDF\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#820000\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewColorfulFixed\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" />");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#990000\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#990000\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#FFCC66\" ForeColor=\"#333333\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#FFFBD6\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#FFCC66\" Font-Bold=\"True\" ForeColor=\"Navy\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FFFDEA\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#4D0000\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#FFFCDF\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#820000\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewColorfulFixedNoPaging\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"True\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" />");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#990000\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#990000\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#FFCC66\" ForeColor=\"#333333\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#FFFBD6\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#FFCC66\" Font-Bold=\"True\" ForeColor=\"Navy\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FFFDEA\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#4D0000\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#FFFCDF\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#820000\" />");
              stringBuilder.AppendLine("</asp:GridView>");
            }
          }
          if (this._appType == ApplicationType.ASPNET || this._appType == ApplicationType.ASPNET45 && this._selectedGridViewStyle == GridViewStyle.Classic)
          {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewClassic\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"" + str2);
            stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
            stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" />");
            stringBuilder.AppendLine("    <EditRowStyle BackColor=\"#2461BF\" />");
            stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#507CD1\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#507CD1\" Font-Bold=\"True\" ForeColor=\"White\" Height=\"26\" />");
            stringBuilder.AppendLine("    <PagerStyle BackColor=\"#A8C4EE\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
            stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
            stringBuilder.AppendLine("    <RowStyle BackColor=\"#EFF3FB\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#D1DDF1\" Font-Bold=\"True\" ForeColor=\"#333333\" />");
            stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#2D59AC\" />");
            stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#7498DA\" />");
            stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F5F7FB\" />");
            stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#E9EBEF\" />");
            stringBuilder.AppendLine("</asp:GridView>");
            if (this._appType == ApplicationType.ASPNET45)
            {
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewClassicNoPagingNoSorting\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"False\"" + str2);
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" />");
              stringBuilder.AppendLine("    <EditRowStyle BackColor=\"#2461BF\" />");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#507CD1\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#507CD1\" Font-Bold=\"True\" ForeColor=\"White\" Height=\"26\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#A8C4EE\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#EFF3FB\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#D1DDF1\" Font-Bold=\"True\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#2D59AC\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#7498DA\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F5F7FB\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#E9EBEF\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewClassicFixed\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" />");
              stringBuilder.AppendLine("    <EditRowStyle BackColor=\"#2461BF\" />");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#507CD1\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#507CD1\" Font-Bold=\"True\" ForeColor=\"White\" Height=\"26\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#A8C4EE\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#EFF3FB\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#D1DDF1\" Font-Bold=\"True\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#2D59AC\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#7498DA\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F5F7FB\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#E9EBEF\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewClassicFixedNoPaging\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"True\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" />");
              stringBuilder.AppendLine("    <EditRowStyle BackColor=\"#2461BF\" />");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#507CD1\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#507CD1\" Font-Bold=\"True\" ForeColor=\"White\" Height=\"26\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#A8C4EE\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#EFF3FB\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#D1DDF1\" Font-Bold=\"True\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#2D59AC\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#7498DA\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F5F7FB\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#E9EBEF\" />");
              stringBuilder.AppendLine("</asp:GridView>");
            }
          }
          if (this._appType == ApplicationType.ASPNET || this._appType == ApplicationType.ASPNET45 && this._selectedGridViewStyle == GridViewStyle.Simple)
          {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewSimple\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"" + str2);
            stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
            stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" />");
            stringBuilder.AppendLine("    <EditRowStyle BackColor=\"#7C6F57\" />");
            stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#1C5E55\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#1C5E55\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <PagerStyle BackColor=\"#CCCCCC\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
            stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
            stringBuilder.AppendLine("    <RowStyle BackColor=\"#E3EAEB\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#C5BBAF\" Font-Bold=\"True\" ForeColor=\"#333333\" />");
            stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F8FAFA\" />");
            stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#246B61\" />");
            stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#EDF0F1\" />");
            stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#15524A\" />");
            stringBuilder.AppendLine("</asp:GridView>");
            if (this._appType == ApplicationType.ASPNET45)
            {
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewSimpleNoPagingNoSorting\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"False\"" + str2);
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" />");
              stringBuilder.AppendLine("    <EditRowStyle BackColor=\"#7C6F57\" />");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#1C5E55\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#1C5E55\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#CCCCCC\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#E3EAEB\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#C5BBAF\" Font-Bold=\"True\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F8FAFA\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#246B61\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#EDF0F1\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#15524A\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewSimpleFixed\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" />");
              stringBuilder.AppendLine("    <EditRowStyle BackColor=\"#7C6F57\" />");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#1C5E55\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#1C5E55\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#CCCCCC\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#E3EAEB\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#C5BBAF\" Font-Bold=\"True\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F8FAFA\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#246B61\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#EDF0F1\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#15524A\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewSimpleFixedNoPaging\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"True\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" />");
              stringBuilder.AppendLine("    <EditRowStyle BackColor=\"#7C6F57\" />");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#1C5E55\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#1C5E55\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#CCCCCC\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#E3EAEB\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#C5BBAF\" Font-Bold=\"True\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F8FAFA\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#246B61\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#EDF0F1\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#15524A\" />");
              stringBuilder.AppendLine("</asp:GridView>");
            }
          }
          if (this._appType == ApplicationType.ASPNET || this._appType == ApplicationType.ASPNET45 && this._selectedGridViewStyle == GridViewStyle.Professional)
          {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewProfessional\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"" + str2);
            stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
            stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" ForeColor=\"#284775\" />");
            stringBuilder.AppendLine("    <EditRowStyle BackColor=\"#999999\" />");
            stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#5D7B9D\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#5D7B9D\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <PagerStyle BackColor=\"#94AFDA\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
            stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
            stringBuilder.AppendLine("    <RowStyle BackColor=\"#F7F6F3\" ForeColor=\"#333333\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#E2DED6\" Font-Bold=\"True\" ForeColor=\"#333333\" />");
            stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F0F0F0\" />");
            stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#506C8C\" />");
            stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#FFFDF8\" />");
            stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#6F8DAE\" />");
            stringBuilder.AppendLine("</asp:GridView>");
            if (this._appType == ApplicationType.ASPNET45)
            {
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewProfessionalNoPagingNoSorting\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"False\"" + str2);
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" ForeColor=\"#284775\" />");
              stringBuilder.AppendLine("    <EditRowStyle BackColor=\"#999999\" />");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#5D7B9D\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#5D7B9D\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#94AFDA\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#F7F6F3\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#E2DED6\" Font-Bold=\"True\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F0F0F0\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#506C8C\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#FFFDF8\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#6F8DAE\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewProfessionalFixed\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" ForeColor=\"#284775\" />");
              stringBuilder.AppendLine("    <EditRowStyle BackColor=\"#999999\" />");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#5D7B9D\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#5D7B9D\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#94AFDA\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#F7F6F3\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#E2DED6\" Font-Bold=\"True\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F0F0F0\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#506C8C\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#FFFDF8\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#6F8DAE\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewProfessionalFixedNoPaging\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"True\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" ForeColor=\"#284775\" />");
              stringBuilder.AppendLine("    <EditRowStyle BackColor=\"#999999\" />");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#5D7B9D\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#5D7B9D\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#94AFDA\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#F7F6F3\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#E2DED6\" Font-Bold=\"True\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F0F0F0\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#506C8C\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#FFFDF8\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#6F8DAE\" />");
              stringBuilder.AppendLine("</asp:GridView>");
            }
          }
          if (this._appType == ApplicationType.ASPNET || this._appType == ApplicationType.ASPNET45 && this._selectedGridViewStyle == GridViewStyle.Autumn)
          {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewAutumn\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"" + str2);
            stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
            stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#FFFFCC\" ForeColor=\"#330099\" />");
            stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#990000\" Font-Bold=\"True\" ForeColor=\"#FFFFCC\" />");
            stringBuilder.AppendLine("    <PagerStyle BackColor=\"#FFFFCC\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
            stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
            stringBuilder.AppendLine("    <RowStyle BackColor=\"White\" ForeColor=\"#330099\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#FFCC66\" Font-Bold=\"True\" ForeColor=\"#663399\" />");
            stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FEFCEB\" />");
            stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#AF0101\" />");
            stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#F6F0C0\" />");
            stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#7E0000\" />");
            stringBuilder.AppendLine("</asp:GridView>");
            if (this._appType == ApplicationType.ASPNET45)
            {
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewAutumnNoPagingNoSorting\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"False\"" + str2);
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#FFFFCC\" ForeColor=\"#330099\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#990000\" Font-Bold=\"True\" ForeColor=\"#FFFFCC\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#FFFFCC\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"White\" ForeColor=\"#330099\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#FFCC66\" Font-Bold=\"True\" ForeColor=\"#663399\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FEFCEB\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#AF0101\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#F6F0C0\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#7E0000\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewAutumnFixed\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#FFFFCC\" ForeColor=\"#330099\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#990000\" Font-Bold=\"True\" ForeColor=\"#FFFFCC\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#FFFFCC\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"White\" ForeColor=\"#330099\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#FFCC66\" Font-Bold=\"True\" ForeColor=\"#663399\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FEFCEB\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#AF0101\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#F6F0C0\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#7E0000\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewAutumnFixedNoPaging\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"True\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#FFFFCC\" ForeColor=\"#330099\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#990000\" Font-Bold=\"True\" ForeColor=\"#FFFFCC\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#FFFFCC\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"White\" ForeColor=\"#330099\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#FFCC66\" Font-Bold=\"True\" ForeColor=\"#663399\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FEFCEB\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#AF0101\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#F6F0C0\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#7E0000\" />");
              stringBuilder.AppendLine("</asp:GridView>");
            }
          }
          if (this._appType == ApplicationType.ASPNET || this._appType == ApplicationType.ASPNET45 && this._selectedGridViewStyle == GridViewStyle.Oceanica)
          {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewOceanica\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"" + str2);
            stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
            stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#99CCCC\" ForeColor=\"#003399\" />");
            stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#003399\" Font-Bold=\"True\" ForeColor=\"#CCCCFF\" />");
            stringBuilder.AppendLine("    <PagerStyle BackColor=\"#99CCCC\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
            stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
            stringBuilder.AppendLine("    <RowStyle BackColor=\"White\" ForeColor=\"#003399\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#009999\" Font-Bold=\"True\" ForeColor=\"#CCFF99\" />");
            stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F2F9F9\" />");
            stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#0D4AC4\" />");
            stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#E9EDED\" />");
            stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#002876\" />");
            stringBuilder.AppendLine("</asp:GridView>");
            if (this._appType == ApplicationType.ASPNET45)
            {
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewOceanicaNoPagingNoSorting\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"False\"" + str2);
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#99CCCC\" ForeColor=\"#003399\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#003399\" Font-Bold=\"True\" ForeColor=\"#CCCCFF\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#99CCCC\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"White\" ForeColor=\"#003399\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#009999\" Font-Bold=\"True\" ForeColor=\"#CCFF99\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F2F9F9\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#0D4AC4\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#E9EDED\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#002876\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewOceanicaFixed\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#99CCCC\" ForeColor=\"#003399\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#003399\" Font-Bold=\"True\" ForeColor=\"#CCCCFF\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#99CCCC\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"White\" ForeColor=\"#003399\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#009999\" Font-Bold=\"True\" ForeColor=\"#CCFF99\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F2F9F9\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#0D4AC4\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#E9EDED\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#002876\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewOceanicaFixedNoPaging\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"True\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#99CCCC\" ForeColor=\"#003399\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#003399\" Font-Bold=\"True\" ForeColor=\"#CCCCFF\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#99CCCC\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"White\" ForeColor=\"#003399\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#009999\" Font-Bold=\"True\" ForeColor=\"#CCFF99\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F2F9F9\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#0D4AC4\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#E9EDED\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#002876\" />");
              stringBuilder.AppendLine("</asp:GridView>");
            }
          }
          if (this._appType == ApplicationType.ASPNET || this._appType == ApplicationType.ASPNET45 && this._selectedGridViewStyle == GridViewStyle.BrownSugar)
          {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewBrownSugar\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"" + str2);
            stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
            stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#F7DFB5\" ForeColor=\"#8C4510\" />");
            stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#A55129\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <PagerStyle BackColor=\"#F2D9CC\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
            stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
            stringBuilder.AppendLine("    <RowStyle BackColor=\"#FFF7E7\" ForeColor=\"#8C4510\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#738A9C\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FFF1D4\" />");
            stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#B95C30\" />");
            stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#F1E5CE\" />");
            stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#93451F\" />");
            stringBuilder.AppendLine("</asp:GridView>");
            if (this._appType == ApplicationType.ASPNET45)
            {
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewBrownSugarNoPagingNoSorting\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"False\"" + str2);
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#F7DFB5\" ForeColor=\"#8C4510\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#A55129\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#F2D9CC\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#FFF7E7\" ForeColor=\"#8C4510\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#738A9C\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FFF1D4\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#B95C30\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#F1E5CE\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#93451F\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewBrownSugarFixed\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#F7DFB5\" ForeColor=\"#8C4510\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#A55129\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#F2D9CC\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#FFF7E7\" ForeColor=\"#8C4510\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#738A9C\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FFF1D4\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#B95C30\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#F1E5CE\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#93451F\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewBrownSugarFixedNoPaging\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"True\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#F7DFB5\" ForeColor=\"#8C4510\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#A55129\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#F2D9CC\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#FFF7E7\" ForeColor=\"#8C4510\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#738A9C\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FFF1D4\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#B95C30\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#F1E5CE\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#93451F\" />");
              stringBuilder.AppendLine("</asp:GridView>");
            }
          }
          if (this._appType == ApplicationType.ASPNET || this._appType == ApplicationType.ASPNET45 && this._selectedGridViewStyle == GridViewStyle.Slate)
          {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewSlate\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"" + str2);
            stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
            stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#B5C7DE\" ForeColor=\"#4A3C8C\" />");
            stringBuilder.AppendLine("    <RowStyle BackColor=\"#E7E7FF\" ForeColor=\"#4A3C8C\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#738A9C\" Font-Bold=\"True\" ForeColor=\"#F7F7F7\" />");
            stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#4A3C8C\" Font-Bold=\"True\" ForeColor=\"#F7F7F7\" />");
            stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"#F7F7F7\" />");
            stringBuilder.AppendLine("    <PagerStyle BackColor=\"#E7E7FF\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
            stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#738A9C\" Font-Bold=\"True\" ForeColor=\"#F7F7F7\" />");
            stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F4F4FD\" />");
            stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#5A4C9D\" />");
            stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#EEEEF9\" />");
            stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#3E3277\" />");
            stringBuilder.AppendLine("</asp:GridView>");
            if (this._appType == ApplicationType.ASPNET45)
            {
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewSlateNoPagingNoSorting\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"False\"" + str2);
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#B5C7DE\" ForeColor=\"#4A3C8C\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#E7E7FF\" ForeColor=\"#4A3C8C\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#738A9C\" Font-Bold=\"True\" ForeColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#4A3C8C\" Font-Bold=\"True\" ForeColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#E7E7FF\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#738A9C\" Font-Bold=\"True\" ForeColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F4F4FD\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#5A4C9D\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#EEEEF9\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#3E3277\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewSlateFixed\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#B5C7DE\" ForeColor=\"#4A3C8C\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#E7E7FF\" ForeColor=\"#4A3C8C\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#738A9C\" Font-Bold=\"True\" ForeColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#4A3C8C\" Font-Bold=\"True\" ForeColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#E7E7FF\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#738A9C\" Font-Bold=\"True\" ForeColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F4F4FD\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#5A4C9D\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#EEEEF9\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#3E3277\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewSlateFixedNoPaging\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"True\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#B5C7DE\" ForeColor=\"#4A3C8C\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#E7E7FF\" ForeColor=\"#4A3C8C\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#738A9C\" Font-Bold=\"True\" ForeColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#4A3C8C\" Font-Bold=\"True\" ForeColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#E7E7FF\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#738A9C\" Font-Bold=\"True\" ForeColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F4F4FD\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#5A4C9D\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#EEEEF9\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#3E3277\" />");
              stringBuilder.AppendLine("</asp:GridView>");
            }
          }
          if (this._appType == ApplicationType.ASPNET || this._appType == ApplicationType.ASPNET45 && this._selectedGridViewStyle == GridViewStyle.SandAndSky)
          {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewSandAndSky\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"" + str2);
            stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
            stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"Tan\" />");
            stringBuilder.AppendLine("    <RowStyle BackColor=\"LightGoldenrodYellow\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"DarkSlateBlue\" ForeColor=\"GhostWhite\" />");
            stringBuilder.AppendLine("    <HeaderStyle BackColor=\"Tan\" Font-Bold=\"True\" />");
            stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"PaleGoldenrod\" />");
            stringBuilder.AppendLine("    <PagerStyle BackColor=\"#E6CEB7\" ForeColor=\"DarkSlateBlue\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
            stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"DarkSlateBlue\" ForeColor=\"GhostWhite\" />");
            stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FAFAE7\" />");
            stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#DAC09E\" />");
            stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#F0F0F0\" />");
            stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#C2A47B\" />");
            stringBuilder.AppendLine("</asp:GridView>");
            if (this._appType == ApplicationType.ASPNET45)
            {
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewSandAndSkyNoPagingNoSorting\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"False\"" + str2);
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"Tan\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"LightGoldenrodYellow\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"DarkSlateBlue\" ForeColor=\"GhostWhite\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"Tan\" Font-Bold=\"True\" />");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"PaleGoldenrod\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#E6CEB7\" ForeColor=\"DarkSlateBlue\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"DarkSlateBlue\" ForeColor=\"GhostWhite\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FAFAE7\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#DAC09E\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#F0F0F0\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#C2A47B\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewSandAndSkyFixed\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"Tan\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"LightGoldenrodYellow\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"DarkSlateBlue\" ForeColor=\"GhostWhite\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"Tan\" Font-Bold=\"True\" />");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"PaleGoldenrod\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#E6CEB7\" ForeColor=\"DarkSlateBlue\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"DarkSlateBlue\" ForeColor=\"GhostWhite\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FAFAE7\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#DAC09E\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#F0F0F0\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#C2A47B\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewSandAndSkyFixedNoPaging\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"True\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"Tan\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"LightGoldenrodYellow\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"DarkSlateBlue\" ForeColor=\"GhostWhite\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"Tan\" Font-Bold=\"True\" />");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"PaleGoldenrod\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#E6CEB7\" ForeColor=\"DarkSlateBlue\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"DarkSlateBlue\" ForeColor=\"GhostWhite\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FAFAE7\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#DAC09E\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#F0F0F0\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#C2A47B\" />");
              stringBuilder.AppendLine("</asp:GridView>");
            }
          }
          if (this._appType == ApplicationType.ASPNET || this._appType == ApplicationType.ASPNET45 && this._selectedGridViewStyle == GridViewStyle.RainyDay)
          {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewRainyDay\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"" + str2);
            stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
            stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#CCCCCC\" ForeColor=\"Black\" />");
            stringBuilder.AppendLine("    <RowStyle BackColor=\"#EEEEEE\" ForeColor=\"Black\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#008A8C\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#000084\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"#DCDCDC\" />");
            stringBuilder.AppendLine("    <PagerStyle BackColor=\"#CCCCCC\" ForeColor=\"Black\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
            stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#008A8C\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F1F1F1\" />");
            stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#0000A9\" />");
            stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CAC9C9\" />");
            stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#000065\" />");
            stringBuilder.AppendLine("</asp:GridView>");
            if (this._appType == ApplicationType.ASPNET45)
            {
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewRainyDayNoPagingNoSorting\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"False\"" + str2);
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#CCCCCC\" ForeColor=\"Black\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#EEEEEE\" ForeColor=\"Black\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#008A8C\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#000084\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"#DCDCDC\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#CCCCCC\" ForeColor=\"Black\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#008A8C\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F1F1F1\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#0000A9\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CAC9C9\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#000065\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewRainyDayFixed\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#CCCCCC\" ForeColor=\"Black\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#EEEEEE\" ForeColor=\"Black\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#008A8C\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#000084\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"#DCDCDC\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#CCCCCC\" ForeColor=\"Black\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#008A8C\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F1F1F1\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#0000A9\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CAC9C9\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#000065\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewRainyDayFixedNoPaging\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"True\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#CCCCCC\" ForeColor=\"Black\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#EEEEEE\" ForeColor=\"Black\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#008A8C\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#000084\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"#DCDCDC\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#CCCCCC\" ForeColor=\"Black\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#008A8C\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F1F1F1\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#0000A9\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CAC9C9\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#000065\" />");
              stringBuilder.AppendLine("</asp:GridView>");
            }
          }
          if (this._appType == ApplicationType.ASPNET || this._appType == ApplicationType.ASPNET45 && this._selectedGridViewStyle == GridViewStyle.SnowPine)
          {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewSnowPine\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"" + str2);
            stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
            stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"White\" ForeColor=\"#000066\" />");
            stringBuilder.AppendLine("    <RowStyle ForeColor=\"#000066\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#669999\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#006699\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <PagerStyle BackColor=\"#F2FBFF\" ForeColor=\"#000066\" HorizontalAlign=\"Left\" CssClass=\"gridviewPagerStyle\" />");
            stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#669999\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F1F1F1\" />");
            stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#007DBB\" />");
            stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CAC9C9\" />");
            stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#00547E\" />");
            stringBuilder.AppendLine("</asp:GridView>");
            if (this._appType == ApplicationType.ASPNET45)
            {
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewSnowPineNoPagingNoSorting\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"False\"" + str2);
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"White\" ForeColor=\"#000066\" />");
              stringBuilder.AppendLine("    <RowStyle ForeColor=\"#000066\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#669999\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#006699\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#F2FBFF\" ForeColor=\"#000066\" HorizontalAlign=\"Left\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#669999\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F1F1F1\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#007DBB\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CAC9C9\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#00547E\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewSnowPineFixed\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"White\" ForeColor=\"#000066\" />");
              stringBuilder.AppendLine("    <RowStyle ForeColor=\"#000066\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#669999\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#006699\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#F2FBFF\" ForeColor=\"#000066\" HorizontalAlign=\"Left\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#669999\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F1F1F1\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#007DBB\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CAC9C9\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#00547E\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewSnowPineFixedNoPaging\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"True\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"White\" ForeColor=\"#000066\" />");
              stringBuilder.AppendLine("    <RowStyle ForeColor=\"#000066\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#669999\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#006699\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#F2FBFF\" ForeColor=\"#000066\" HorizontalAlign=\"Left\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#669999\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F1F1F1\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#007DBB\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CAC9C9\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#00547E\" />");
              stringBuilder.AppendLine("</asp:GridView>");
            }
          }
          if (this._appType == ApplicationType.ASPNET || this._appType == ApplicationType.ASPNET45 && this._selectedGridViewStyle == GridViewStyle.LilacsInMist)
          {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewLilacsInMist\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"" + str2);
            stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
            stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#C6C3C6\" ForeColor=\"Black\" />");
            stringBuilder.AppendLine("    <RowStyle BackColor=\"#DEDFDE\" ForeColor=\"Black\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#9471DE\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#4A3C8C\" Font-Bold=\"True\" ForeColor=\"#E7E7FF\" />");
            stringBuilder.AppendLine("    <PagerStyle BackColor=\"#C6C3C6\" ForeColor=\"Black\" HorizontalAlign=\"Right\" CssClass=\"gridviewPagerStyle\" />");
            stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#9471DE\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F1F1F1\" />");
            stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#594B9C\" />");
            stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CAC9C9\" />");
            stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#33276A\" />");
            stringBuilder.AppendLine("</asp:GridView>");
            if (this._appType == ApplicationType.ASPNET45)
            {
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewLilacsInMistNoPagingNoSorting\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"False\"" + str2);
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#C6C3C6\" ForeColor=\"Black\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#DEDFDE\" ForeColor=\"Black\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#9471DE\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#4A3C8C\" Font-Bold=\"True\" ForeColor=\"#E7E7FF\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#C6C3C6\" ForeColor=\"Black\" HorizontalAlign=\"Right\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#9471DE\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F1F1F1\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#594B9C\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CAC9C9\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#33276A\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewLilacsInMistFixed\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#C6C3C6\" ForeColor=\"Black\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#DEDFDE\" ForeColor=\"Black\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#9471DE\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#4A3C8C\" Font-Bold=\"True\" ForeColor=\"#E7E7FF\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#C6C3C6\" ForeColor=\"Black\" HorizontalAlign=\"Right\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#9471DE\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F1F1F1\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#594B9C\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CAC9C9\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#33276A\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewLilacsInMistFixedNoPaging\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"True\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#C6C3C6\" ForeColor=\"Black\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#DEDFDE\" ForeColor=\"Black\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#9471DE\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#4A3C8C\" Font-Bold=\"True\" ForeColor=\"#E7E7FF\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#C6C3C6\" ForeColor=\"Black\" HorizontalAlign=\"Right\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#9471DE\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F1F1F1\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#594B9C\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CAC9C9\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#33276A\" />");
              stringBuilder.AppendLine("</asp:GridView>");
            }
          }
          if (this._appType == ApplicationType.ASPNET || this._appType == ApplicationType.ASPNET45 && this._selectedGridViewStyle == GridViewStyle.BlackAndBlue)
          {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewBlackAndBlue\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"" + str2);
            stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
            stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#CCCCCC\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#000099\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <HeaderStyle BackColor=\"Black\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"#CCCCCC\" />");
            stringBuilder.AppendLine("    <PagerStyle BackColor=\"#CCCCCC\" ForeColor=\"Black\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
            stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#000099\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F1F1F1\" />");
            stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#808080\" />");
            stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CAC9C9\" />");
            stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#383838\" />");
            stringBuilder.AppendLine("</asp:GridView>");
            if (this._appType == ApplicationType.ASPNET45)
            {
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewBlackAndBlueNoPagingNoSorting\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"False\"" + str2);
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#CCCCCC\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#000099\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"Black\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"#CCCCCC\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#CCCCCC\" ForeColor=\"Black\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#000099\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F1F1F1\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#808080\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CAC9C9\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#383838\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewBlackAndBlueFixed\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#CCCCCC\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#000099\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"Black\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"#CCCCCC\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#CCCCCC\" ForeColor=\"Black\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#000099\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F1F1F1\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#808080\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CAC9C9\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#383838\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewBlackAndBlueFixedNoPaging\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"True\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#CCCCCC\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#000099\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"Black\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"#CCCCCC\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#CCCCCC\" ForeColor=\"Black\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#000099\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F1F1F1\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#808080\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CAC9C9\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#383838\" />");
              stringBuilder.AppendLine("</asp:GridView>");
            }
          }
          if (this._appType == ApplicationType.ASPNET || this._appType == ApplicationType.ASPNET45 && this._selectedGridViewStyle == GridViewStyle.CloverField)
          {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewCloverField\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"" + str2);
            stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
            stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"White\" ForeColor=\"#333333\" />");
            stringBuilder.AppendLine("    <RowStyle BackColor=\"White\" ForeColor=\"#333333\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#339966\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#336666\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <PagerStyle BackColor=\"#EEFDE1\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
            stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#339966\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F7F7F7\" />");
            stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#487575\" />");
            stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CCCCCC\" />");
            stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#275353\" />");
            stringBuilder.AppendLine("</asp:GridView>");
            if (this._appType == ApplicationType.ASPNET45)
            {
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewCloverFieldNoPagingNoSorting\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"False\"" + str2);
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"White\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"White\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#339966\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#336666\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#EEFDE1\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#339966\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#487575\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CCCCCC\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#275353\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewCloverFieldFixed\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"White\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"White\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#339966\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#336666\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#EEFDE1\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#339966\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#487575\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CCCCCC\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#275353\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewCloverFieldFixedNoPaging\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"True\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"White\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"White\" ForeColor=\"#333333\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#339966\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#336666\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#EEFDE1\" HorizontalAlign=\"Center\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#339966\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#487575\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CCCCCC\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#275353\" />");
              stringBuilder.AppendLine("</asp:GridView>");
            }
          }
          if (this._appType == ApplicationType.ASPNET || this._appType == ApplicationType.ASPNET45 && this._selectedGridViewStyle == GridViewStyle.AppleOrchard)
          {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewAppleOrchard\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"" + str2);
            stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
            stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#CCCC99\" ForeColor=\"Black\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#CC3333\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#333333\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <PagerStyle BackColor=\"#EFEFEF\" ForeColor=\"Black\" HorizontalAlign=\"Right\" CssClass=\"gridviewPagerStyle\" />");
            stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#CC3333\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F7F7F7\" />");
            stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#4B4B4B\" />");
            stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CCCCCC\" />");
            stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#242121\" />");
            stringBuilder.AppendLine("</asp:GridView>");
            if (this._appType == ApplicationType.ASPNET45)
            {
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewAppleOrchardNoPagingNoSorting\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"False\"" + str2);
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#CCCC99\" ForeColor=\"Black\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#CC3333\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#333333\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#EFEFEF\" ForeColor=\"Black\" HorizontalAlign=\"Right\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#CC3333\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#4B4B4B\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CCCCCC\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#242121\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewAppleOrchardFixed\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#CCCC99\" ForeColor=\"Black\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#CC3333\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#333333\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#EFEFEF\" ForeColor=\"Black\" HorizontalAlign=\"Right\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#CC3333\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#4B4B4B\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CCCCCC\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#242121\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewAppleOrchardFixedNoPaging\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"True\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#CCCC99\" ForeColor=\"Black\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#CC3333\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#333333\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#EFEFEF\" ForeColor=\"Black\" HorizontalAlign=\"Right\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#CC3333\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#F7F7F7\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#4B4B4B\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#CCCCCC\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#242121\" />");
              stringBuilder.AppendLine("</asp:GridView>");
            }
          }
          if (this._appType == ApplicationType.ASPNET || this._appType == ApplicationType.ASPNET45 && this._selectedGridViewStyle == GridViewStyle.Mocha)
          {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewMocha\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"" + str2);
            stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
            stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#CCCC99\" />");
            stringBuilder.AppendLine("    <RowStyle BackColor=\"#F7F7DE\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#CE5D5A\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#6B696B\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" />");
            stringBuilder.AppendLine("    <PagerStyle BackColor=\"#F4F4D5\" ForeColor=\"Black\" HorizontalAlign=\"Right\" CssClass=\"gridviewPagerStyle\" />");
            stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
            stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#CE5D5A\" Font-Bold=\"True\" ForeColor=\"White\" />");
            stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FBFBF2\" />");
            stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#848384\" />");
            stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#EAEAD3\" />");
            stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#575357\" />");
            stringBuilder.AppendLine("</asp:GridView>");
            if (this._appType == ApplicationType.ASPNET45)
            {
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewMochaNoPagingNoSorting\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"False\"" + str2);
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#CCCC99\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#F7F7DE\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#CE5D5A\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#6B696B\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#F4F4D5\" ForeColor=\"Black\" HorizontalAlign=\"Right\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#CE5D5A\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FBFBF2\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#848384\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#EAEAD3\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#575357\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewMochaFixed\" runat=\"server\" AllowPaging=\"True\" AllowSorting=\"True\" PageSize=\"16\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#CCCC99\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#F7F7DE\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#CE5D5A\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#6B696B\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#F4F4D5\" ForeColor=\"Black\" HorizontalAlign=\"Right\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#CE5D5A\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FBFBF2\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#848384\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#EAEAD3\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#575357\" />");
              stringBuilder.AppendLine("</asp:GridView>");
              stringBuilder.AppendLine("");
              stringBuilder.AppendLine("<asp:GridView SkinID=\"GridViewMochaFixedNoPaging\" runat=\"server\" AllowPaging=\"False\" AllowSorting=\"True\"");
              stringBuilder.AppendLine("    AutoGenerateColumns=\"False\" CellPadding=\"" + str1 + "\" ForeColor=\"Black\" GridLines=\"Both\" CssClass=\"gridviewGridLines\">");
              stringBuilder.AppendLine("    <FooterStyle HorizontalAlign=\"Right\"  BackColor=\"#CCCC99\" />");
              stringBuilder.AppendLine("    <RowStyle BackColor=\"#F7F7DE\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#CE5D5A\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <HeaderStyle BackColor=\"#6B696B\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <AlternatingRowStyle BackColor=\"White\" />");
              stringBuilder.AppendLine("    <PagerStyle BackColor=\"#F4F4D5\" ForeColor=\"Black\" HorizontalAlign=\"Right\" CssClass=\"gridviewPagerStyle\" />");
              stringBuilder.AppendLine("    <PagerSettings PageButtonCount=\"10\" Mode=\"NumericFirstLast\" FirstPageText=\"< First\" LastPageText=\"Last >\" />");
              stringBuilder.AppendLine("    <SelectedRowStyle BackColor=\"#CE5D5A\" Font-Bold=\"True\" ForeColor=\"White\" />");
              stringBuilder.AppendLine("    <SortedAscendingCellStyle BackColor=\"#FBFBF2\" />");
              stringBuilder.AppendLine("    <SortedAscendingHeaderStyle BackColor=\"#848384\" />");
              stringBuilder.AppendLine("    <SortedDescendingCellStyle BackColor=\"#EAEAD3\" />");
              stringBuilder.AppendLine("    <SortedDescendingHeaderStyle BackColor=\"#575357\" />");
              stringBuilder.AppendLine("</asp:GridView>");
            }
          }
        }
        streamWriter.Write(stringBuilder.ToString());
      }
    }
  }
}
