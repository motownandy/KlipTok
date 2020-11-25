using System;

namespace KlipTok.Client.Formatters {

  public static class Number {

    public static string ToShortString(this long value) {

      string outVal = "";
      if (value >= 750_000_000_000) {
        var d = ((double)value)/1_000_000_000_000d;
        outVal = Math.Round(d, 2).ToString() + "t";
      } else if (value >= 750_000_000) {
        var d = ((double)value)/1_000_000_000d;
        outVal = Math.Round(d, 2).ToString() + "b";
      } else if (value >= 750_000) {
        var d = ((double)value)/1_000_000d;
        outVal = Math.Round(d, 2).ToString() + "m";
      } else if (value >= 750) {
        var d = ((double)value)/1_000d;
        outVal = Math.Round(d, 2).ToString() + "k";
      }
 
      return outVal;

    }

    public static string ToShortString(this int value) {
      return ((long)value).ToShortString();
    }

  }

}