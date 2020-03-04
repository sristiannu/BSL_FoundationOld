
namespace KPIT_K_Foundation
{
  internal sealed class RequiredError
  {
    internal bool IsInSelectedTablesTab { get; set; }

    internal bool IsInSelectedViewsTab { get; set; }

    internal bool IsInDatabaseSettingsTab { get; set; }

    internal bool IsInCodeSettingsTab { get; set; }

    internal bool IsInUISettingsTab { get; set; }

    internal bool IsInAppSettingsTab { get; set; }

    internal bool IsSelectedTableBlank { get; set; }

    internal bool IsSelectedViewsBlank { get; set; }

    internal bool IsServerBlank { get; set; }

    internal bool IsDatabaseBlank { get; set; }

    internal bool IsUserNameBlank { get; set; }

    internal bool IsPasswordBlank { get; set; }

    internal bool IsSpPrefixBlank { get; set; }

    internal bool IsSpSuffixBlank { get; set; }

    internal bool IsWebsiteNameBlank { get; set; }

    internal bool IsWebsiteDirectoryBlank { get; set; }

    internal bool IsNameSpaceBlank { get; set; }

    internal bool IsViewListCrudRedirectBlank { get; set; }

    internal bool IsViewAddRecordBlank { get; set; }

    internal bool IsViewUpdateRecordBlank { get; set; }

    internal bool IsViewRecordDetailsBlank { get; set; }

    internal bool IsViewListReadOnlyBlank { get; set; }

    internal bool IsViewListCrudBlank { get; set; }

    internal bool IsViewListGroupedByBlank { get; set; }

    internal bool IsViewListTotalsBlank { get; set; }

    internal bool IsViewListTotalsGroupedByBlank { get; set; }

    internal bool IsViewListSearchBlank { get; set; }

    internal bool IsViewListScrollLoadBlank { get; set; }

    internal bool IsViewListInlineBlank { get; set; }

    internal bool IsViewListForeachBlank { get; set; }

    internal bool IsViewUnboundBlank { get; set; }

    internal bool IsAppFilesDirectoryBlank { get; set; }

    private RequiredError()
    {
    }
  }
}
