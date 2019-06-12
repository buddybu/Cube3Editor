using SourceGrid;
using System;

namespace Cube3Editor
{
    public class RetractionStopChangedEvent : SourceGrid.Cells.Controllers.ControllerBase
    {
        private MainEditor mFrm;
        public RetractionStopChangedEvent(MainEditor frm)
        {
            mFrm = frm;
        }

        public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnValueChanged(sender, e);
            mFrm.UpdateStopRetractions((Grid)sender.Grid);
        }
    }

}
