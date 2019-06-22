﻿using SourceGrid;
using System;

namespace Cube3Editor
{
    public class PressureChangedEvent : SourceGrid.Cells.Controllers.ControllerBase
    {
        private MainEditor mFrm;
        public PressureChangedEvent(MainEditor frm)
        {
            mFrm = frm;
        }

        public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnValueChanged(sender, e);
            mFrm.UpdatePressures();
        }
    }

}
