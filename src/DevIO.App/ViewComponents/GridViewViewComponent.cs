using DevIO.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevIO.App.ViewComponents
{
    [ViewComponent(Name = "GridView")]
    public class GridViewViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string[] headerColumns, 
            string[] displayColumns, string[] columnType, string url, string controller, 
            bool readOnly = false, int pageSize = 10, bool exporting = false)
        {
            GridViewViewModel gridView = new GridViewViewModel
            {
                HeaderColumns = headerColumns,
                DisplayColumns = displayColumns,
                ColumnType = columnType,
                Url = url,
                Controller = controller,
                ReadOnly = readOnly,
                PageSize = pageSize,
                Exporting = exporting
            };
            return View(gridView);
        }
    }
}
