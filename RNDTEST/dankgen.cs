﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNDTEST
{
  class dankgen
  {
    public void Generator(int key)
    {
      int[,] plate = new int[3, 9];
      dankclass420 dnk = new dankclass420();

      for (int count = 0; count < 9; count++)
      {
        int[] column = new int[3];
        if (count == 0)
        {
          column = dnk.FillColumn(1, 9, key, count);
        }
        if (count == 8)
        {
          column = dnk.FillColumn(80, 90, key, count);
        }
        else
        {
          int lower, upper;
          lower = count * 10;
          upper = lower + 9;
          column = dnk.FillColumn(lower, upper, key, count);
        }
        for (int i = 0; i < 3; i++)
        {
          plate[i, count] = column[i];
        }
      }


      for (int i = 0; i < 3; i++)
      {
        for (int j = 0; j < 9; j++)
        {
          Console.Write(plate[i, j] + " | ");
        }
        Console.WriteLine();
      }
      Console.WriteLine();
    }
  }
}