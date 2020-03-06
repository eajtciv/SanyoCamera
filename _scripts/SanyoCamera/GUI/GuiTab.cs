using System;
using System.Collections.Generic;
using UnityEngine;
using SanyoCamera;
using SanyoLib;

namespace SanyoCamera.GUI
{
  public class GuiTab : List<IGuiTab>
  {
    public int Index { set; get; }
    private int Offset = 0;
    public int Limit { set; get; } = 3;

    public void Mount(GUIStyleManager style){
      int size = Math.Min(this.Count, this.Limit);
      IGuiTab[] items = new IGuiTab[size];
      Array.Copy(this.ToArray(),this.Offset, items, 0, size);
      GUILayout.BeginHorizontal();
      this.Index = this.Offset+GUILayout.SelectionGrid(Math.Min(Math.Max(this.Index-this.Offset,0),size-1), Array.ConvertAll(items, i => i.Name()), size, style["tab"]);
      if(this.Count != size){
        bool prev = this.Offset-1 >= 0;
        bool next = this.Offset+1 <= this.Count-this.Limit;
        if(GUILayout.Button("<", style["button"+(prev?"":"-disable")], GUILayout.Width(30)) && prev)
          this.Offset--;
        if(GUILayout.Button(">", style["button"+(next?"":"-disable")], GUILayout.Width(30)) && next)
          this.Offset++;
      }
      GUILayout.EndHorizontal();
      GUILayout.BeginVertical();
      GUILayout.Space(3);
      try{
        this[this.Index].Mount();
      }catch(Exception e){
        GUILayout.Label(e.ToString());
      }
      GUILayout.EndVertical();
    }
  }
}