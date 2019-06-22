using BitForByteSupport;
using DevAge.Drawing;
using SourceGrid;
using System;
using System.Windows.Forms;

namespace Cube3Editor
{
    public partial class MainEditor
    {
        internal void PopulateRetractionStarts(RectangleBorder border)
        {

            //List<string> retractionStartLines = bfbObject.generateRetractionStartList();

            SourceGrid.Cells.Editors.TextBox retractCountEditor = new SourceGrid.Cells.Editors.TextBox(typeof(int));
            retractCountEditor.EditableMode = SourceGrid.EditableMode.None;


            int gridRow = 1;
            RetractionStartChangedEvent valueChangedController = new RetractionStartChangedEvent(this);


            foreach (string key in bfbObject.RetractionStartLineList.Keys)
            {
                int index = bfbObject.RetractionStartLineList[key][0];
                Retraction retraction = bfbObject.RetractionStartDictionary[index];

                gridRetractionStart.Rows.Insert(gridRow);
                gridRetractionStart[gridRow, 0] = new SourceGrid.Cells.Cell(retraction.P, typeof(int));
                gridRetractionStart[gridRow, 0].AddController(valueChangedController);
                gridRetractionStart[gridRow, 1] = new SourceGrid.Cells.Cell(retraction.S, typeof(int));
                gridRetractionStart[gridRow, 1].AddController(valueChangedController);
                if (retraction.G != -1)
                {
                    gridRetractionStart[gridRow, 2] = new SourceGrid.Cells.Cell(retraction.G, typeof(int));
                    gridRetractionStart[gridRow, 2].AddController(valueChangedController);
                }
                else
                {
                    gridRetractionStart[gridRow, 2] = new SourceGrid.Cells.Cell(" ", retractCountEditor);
                }
                if (retraction.F != -1)
                {
                    gridRetractionStart[gridRow, 3] = new SourceGrid.Cells.Cell(retraction.F, typeof(int));
                    gridRetractionStart[gridRow, 3].AddController(valueChangedController);
                }
                else
                {
                    gridRetractionStart[gridRow, 3] = new SourceGrid.Cells.Cell(" ", retractCountEditor);
                }
                gridRetractionStart[gridRow, 4] = new SourceGrid.Cells.Cell(index, retractCountEditor);

                //gridLeftTemps[gridRow, 1] = new SourceGrid.Cells.CellControl(); 
                gridRow++;
            }
        }

        internal void PopulateRetractionStops(RectangleBorder border)
        {

            //List<string> retractionStartLines = bfbObject.generateRetractionStartList();

            SourceGrid.Cells.Editors.TextBox retractCountEditor = new SourceGrid.Cells.Editors.TextBox(typeof(int));
            retractCountEditor.EditableMode = SourceGrid.EditableMode.None;


            int gridRow = 1;
            RetractionStopChangedEvent valueChangedController = new RetractionStopChangedEvent(this);


            foreach (string key in bfbObject.RetractionStopLineList.Keys)
            {
                int index = bfbObject.RetractionStopLineList[key][0];
                Retraction retraction = bfbObject.RetractionStopDictionary[index];

                gridRetractionStop.Rows.Insert(gridRow);
                gridRetractionStop[gridRow, 0] = new SourceGrid.Cells.Cell(retraction.P, typeof(int));
                gridRetractionStop[gridRow, 0].AddController(valueChangedController);
                gridRetractionStop[gridRow, 1] = new SourceGrid.Cells.Cell(retraction.S, typeof(int));
                gridRetractionStop[gridRow, 1].AddController(valueChangedController);
                gridRetractionStop[gridRow, 2] = new SourceGrid.Cells.Cell(index, retractCountEditor);

                gridRow++;
            }
        }

        internal void UpdateStartRetractions(Grid gridStartRetracts)
        {
            if (gridStartRetracts.Rows.Count > 1)
            {
                for (int i = 1; i < gridStartRetracts.Rows.Count; i++)
                {
                    int p = (int)gridStartRetracts[i, 0].Value;
                    int s = (int)gridStartRetracts[i, 1].Value;
                    int g = (int)gridStartRetracts[i, 2].Value;
                    int f = (int)gridStartRetracts[i, 3].Value;
                    int index = (int)gridStartRetracts[i, 4].Value;

                    string retractCmd = BFBConstants.RETRACT_START + " P" + p + " S" + s;
                    if (g != -1)
                        retractCmd += " G" + g;

                    if (f != -1)
                        retractCmd += " F" + f;

                    bfbObject.updateRetractionStartLines(index, retractCmd);

                }
            }
        }

        internal void UpdateStopRetractions(Grid gridStopRetracts)
        {
            if (gridStopRetracts.Rows.Count > 1)
            {
                for (int i = 1; i < gridStopRetracts.Rows.Count; i++)
                {
                    int p = (int)gridStopRetracts[i, 0].Value;
                    int s = (int)gridStopRetracts[i, 1].Value;
                    int index = (int)gridStopRetracts[i, 2].Value;

                    string retractCmd = BFBConstants.RETRACT_STOP + " P" + p + " S" + s;

                    bfbObject.updateRetractionStopLines(index, retractCmd);

                }
            }
        }
    }
}