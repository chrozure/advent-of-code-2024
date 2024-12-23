using System;

class Robot(int x, int y, int xVel, int yVel)
{

    public string color = "red";
    public int XPos = x;
    public int YPos = y;
    public int XVel = xVel;
    public int YVel = yVel;

    public void Move(int rows, int cols) {
        XPos += XVel;
        if (XPos < 0) XPos += cols;
        if (XPos >= cols) XPos -= cols;

        YPos += YVel;
        if (YPos < 0) YPos += rows;
        if (YPos >= rows) YPos -= rows;
    }


}