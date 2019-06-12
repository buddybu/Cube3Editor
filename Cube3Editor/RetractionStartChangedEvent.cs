using SourceGrid;
using System;

namespace Cube3Editor
{
    public class RetractionStartChangedEvent : SourceGrid.Cells.Controllers.ControllerBase
    {
        private MainEditor mFrm;
        public RetractionStartChangedEvent(MainEditor frm)
        {
            mFrm = frm;
        }

        public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnValueChanged(sender, e);
            mFrm.UpdateStartRetractions((Grid)sender.Grid);
        }
    }

}
