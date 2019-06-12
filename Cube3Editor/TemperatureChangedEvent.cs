using SourceGrid;
using System;

namespace Cube3Editor
{
    public class TemperatureChangedEvent : SourceGrid.Cells.Controllers.ControllerBase
    {
        private MainEditor mFrm;
        public TemperatureChangedEvent(MainEditor frm)
        {
            mFrm = frm;
        }

        public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnValueChanged(sender, e);
            mFrm.CalculateTemperatures((Grid)sender.Grid);
        }

    }

}
