using ExcelReport.Common;
using ExcelReport.Contexts;
using ExcelReport.Driver;
using ExcelReport.Exceptions;
using ExcelReport.Extends;
using ExcelReport.Meta;
using System;
using System.Linq;

namespace ExcelReport.Renderers
{
	public class ParameterRenderer : Named, IElementRenderer
	{
		protected object Value { set; get; }

		public ParameterRenderer(string name, object value)
		{
			Name = name;
			Value = value;
		}

		public int SortNum(SheetContext sheetContext)
		{
			Parameter parameter = sheetContext.WorksheetContainer.Parameters[Name];
			return parameter.Locations.Min(location => location.RowIndex);
		}

		public virtual void Render(SheetContext sheetContext)
		{
			Parameter parameter = sheetContext.WorksheetContainer.Parameters[Name];
			foreach (var location in parameter.Locations)
			{
				ICell cell = sheetContext.GetCell(location);
				if (null == cell)
				{
					throw new RenderException($"parameter[{parameter.Name}],cell[{location.RowIndex},{location.ColumnIndex}] is null");
				}
				var parameterName = $"$[{parameter.Name}]";
				if (parameterName.Equals(cell.GetStringValue().Trim()))
				{
					cell.Value = Value;
				}
				else
				{
					cell.Value = (cell.GetStringValue().Replace(parameterName, Value.CastTo<string>()));
				}
			}
		}
	}

	public class ParameterRenderer<TSource> : Named, IEmbeddedRenderer<TSource>
	{
		protected Func<TSource, object> DgSetValue { set; get; }

		public ParameterRenderer(string name, Func<TSource, object> dgSetValue)
		{
			Name = name;
			DgSetValue = dgSetValue;
		}

		public int SortNum(SheetContext sheetContext)
		{
			Parameter parameter = sheetContext.WorksheetContainer.Parameters[Name];
			if (parameter.Locations.Count == 0) return 0;
			return parameter.Locations.Min(location => location.RowIndex);
		}

		public virtual void Render(SheetContext sheetContext, TSource dataSource)
		{
			Parameter parameter = sheetContext.WorksheetContainer.Parameters[Name];
			foreach (var location in parameter.Locations)
			{
				ICell cell = sheetContext.GetCell(location);
				if (null == cell)
				{
					throw new RenderException($"parameter[{parameter.Name}],cell[{location.RowIndex},{location.ColumnIndex}] is null");
				}

				var parameterName = $"$[{parameter.Name}]";
				var value = DgSetValue(dataSource);
				if (parameterName.Equals(cell.GetStringValue().Trim()))
				{
					cell.Value = value;
				}
				else
				{
					cell.Value = cell.GetStringValue().Replace($"$[{parameter.Name}]", value.CastTo<string>());
				}
			}
		}
	}
}