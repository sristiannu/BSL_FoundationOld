
using System;
using System.Windows.Forms;

namespace KPIT_K_Foundation
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      int num2 = (int)new AutoCodeGenerator().ShowDialog();
            Application.Run(new AutoCodeGenerator());
    }
  }
}
