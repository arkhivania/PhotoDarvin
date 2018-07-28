using Nailhang.MVVM;
using Photo.PrintTool.AreaLayouts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.PrintTool.LayoutsTool.ViewModel
{
    class SetupLayout : IDisposable
    {
        private readonly IAreaLayouts areaLayouts;

        public CollectionWithCurrentItem<Base.Layout> Layouts { get; } = new CollectionWithCurrentItem<Base.Layout>();
        public CollectionWithCurrentItem<BorderDisplay> Borders { get; } = new CollectionWithCurrentItem<BorderDisplay>();

        public SetupLayout(IAreaLayouts areaLayouts)
        {
            this.areaLayouts = areaLayouts;

            Borders.AddItem(new BorderDisplay { Name = "No borders", BorderSize = 0 });
            Borders.AddItem(new BorderDisplay { Name = "Small borders", BorderSize = 0.0025 });
            Borders.AddItem(new BorderDisplay { Name = "Medium borders", BorderSize = 0.005 });
            Borders.AddItem(new BorderDisplay { Name = "Large borders", BorderSize = 0.010 });

            Layouts.AddItem(new Base.Layout { Rows = 1, Columns = 1 });
            Layouts.AddItem(new Base.Layout { Rows = 1, Columns = 2 });
            Layouts.AddItem(new Base.Layout { Rows = 2, Columns = 1 });
            Layouts.AddItem(new Base.Layout { Rows = 2, Columns = 2 });
            Layouts.AddItem(new Base.Layout { Rows = 3, Columns = 3 });
            Layouts.AddItem(new Base.Layout { Rows = 4, Columns = 2 });
            Layouts.AddItem(new Base.Layout { Rows = 2, Columns = 4 });
            Layouts.AddItem(new Base.Layout { Rows = 4, Columns = 4 });

            Borders.Value = Borders.Skip(1).First();
            Layouts.Value = Layouts.Skip(3).First();

            Borders.ValueChanged += Borders_ValueChanged;
            Layouts.ValueChanged += Layouts_ValueChanged;

            UpdateAreas();            
        }

        private void Borders_ValueChanged(object sender, EventArgs e)
        {
            UpdateAreas();
        }

        private void Layouts_ValueChanged(object sender, EventArgs e)
        {
            UpdateAreas();
        }

        private void UpdateAreas()
        {
            var l = Layouts.Value;

            var areas = new List<Area>();
            var cell_width = 1.0 / l.Columns;
            var cell_height = 1.0 / l.Rows;

            for (int j = 0; j < l.Rows; ++j)
                for(int i = 0; i < l.Columns; ++i)
                    areas.Add(new Area { Left = cell_width * i, Top = cell_height * j, Width = cell_width, Height = cell_height, Id = j * l.Columns + i });

            areaLayouts.LayoutState.Value = new Layout()
            {
                Areas = areas.ToArray(),
                BorderSize = Borders.Value.BorderSize
            };
        }

        public void Dispose()
        {
            Borders.ValueChanged -= Borders_ValueChanged;
            Layouts.ValueChanged -= Layouts_ValueChanged;
        }
    }
}
