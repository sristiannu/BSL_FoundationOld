
using System;

namespace KPIT_K_Foundation
{
  internal sealed class GenerateProductSerial
  {
    internal string SerialNumber()
    {
      string str = Guid.NewGuid().ToString();
      return MySettings.AppActivationFirstSevenCharacters + str.Insert(str.IndexOf("-") + 1, "JV").ToUpper() + MySettings.AppActivationFiveLastCharacters;
    }

    internal string ActivationCode(string serialNumber)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      foreach (char ch in serialNumber)
        empty2 += this.ConvertSerial(ch.ToString());
      return empty2;
    }

    internal string ConvertSerial(string character)
    {
      switch (character.ToLower())
      {
        case "-":
          return "-";
        case "0":
          return "I";
        case "1":
          return "J";
        case "2":
          return "H";
        case "3":
          return "F";
        case "4":
          return "G";
        case "5":
          return "E";
        case "6":
          return "D";
        case "7":
          return "C";
        case "8":
          return "A";
        case "9":
          return "B";
        case "a":
          return "0";
        case "b":
          return "Z";
        case "c":
          return "1";
        case "d":
          return "Y";
        case "e":
          return "2";
        case "f":
          return "X";
        case "g":
          return "3";
        case "h":
          return "W";
        case "i":
          return "4";
        case "j":
          return "V";
        case "k":
          return "5";
        case "l":
          return "U";
        case "m":
          return "6";
        case "n":
          return "T";
        case "o":
          return "7";
        case "p":
          return "9";
        case "q":
          return "8";
        case "r":
          return "R";
        case "s":
          return "Q";
        case "t":
          return "S";
        case "u":
          return "O";
        case "v":
          return "P";
        case "w":
          return "N";
        case "x":
          return "L";
        case "y":
          return "M";
        case "z":
          return "K";
        default:
          return "";
      }
    }
  }
}
