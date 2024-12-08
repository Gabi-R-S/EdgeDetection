// See https://aka.ms/new-console-template for more information
using SFML.Graphics;
using SFML.Window;
using SFML.System;
static class Start 
{
    static Image ToGrayscale(Image image) 
    {
        var newImage = new Image(image.Size.X, image.Size.Y);

        for (uint x = 0; x < image.Size.X; x++)
        {
            for (uint y = 0; y < image.Size.Y; y++) 
            {
                var pixel=image.GetPixel(x,y);
                int sum = pixel.R + pixel.G + pixel.B;
                byte brightness = (byte)(sum / 3);
                Color newColor = new Color(brightness,brightness,brightness,pixel.A);
                newImage.SetPixel(x,y,newColor);
            }
        }

        return newImage;
    }

    static Image SobelFilter(Image image) 
    {

        Image newImage = new Image(image.Size.X,image.Size.Y);

        var g1 = new int[,] { {-1,-0,1 }, {-2,0,2 }, {-1,0,1 } };
        var g2 = new int[,] { {-1,-2,-1 }, {0,0,0 }, { 1,2,1} };
        for (int x=1;x<image.Size.X-2;x++) 
        {
            for (int y = 1; y < image.Size.Y - 2; y++)
            {
                int sum1=0;
                int sum2=0;
                for (int i = -1; i <= 1; i++) 
                {
                    for (int j = -1; j <= 1; j++) 
                    {
                        sum1 += g1[1 + i, 1 + j] * (int)image.GetPixel((uint)(x +i), (uint)(y +j)).R;
                        sum2 += g1[1 + i, 1 + j] * (int)image.GetPixel((uint)(x  + i), (uint)(y  + j)).R;
                       
                    }
                }

                byte newB =(byte)(Math.Sqrt(Math.Pow(sum1,2)+ Math.Pow(sum2, 2)));
                Color newColor = new Color(newB,newB,newB,image.GetPixel((uint)x, (uint)y).A) ;
                newImage.SetPixel((uint)x, (uint)y,newColor);
            }
        }

        return newImage;
    }
    static void Close(object? sender,EventArgs args) 
    {
        var window = sender as Window;
        if (window != null) { window.Close(); } 
    }
    public static void Main() 
    {
        Image image = new Image("image.png");
        var grayScale=ToGrayscale(image);
        var filtered=SobelFilter(image);
        
        
        RenderWindow window = new RenderWindow(new VideoMode(1000,1000),"Edge Detection");
        window.Closed += Close;

        Sprite spriteNoFilter = new Sprite(new Texture(image));
        spriteNoFilter.Texture.Smooth = true;
        spriteNoFilter.Scale = new Vector2f(1000.0f / (float)image.Size.X, 1000.0f / (float)image.Size.Y);


        Sprite spriteGrayScale = new Sprite(new Texture(grayScale));
        spriteGrayScale.Texture.Smooth = true;
        spriteGrayScale.Scale = new Vector2f(1000.0f / (float)image.Size.X, 1000.0f / (float)image.Size.Y);

        Sprite spriteFilter = new Sprite(new Texture(filtered));
        spriteFilter.Texture.Smooth = true;
        spriteFilter.Scale=new Vector2f(1000.0f/(float)image.Size.X, 1000.0f / (float)image.Size.Y);

        int drawFilter = 2;
        System.EventHandler<KeyEventArgs> press = (object? sender, KeyEventArgs args) =>
        {
            if (args.Code == Keyboard.Key.Space) { drawFilter +=1; if (drawFilter == 3) { drawFilter = 0; } }
        };
        window.KeyPressed += press;
        while (window.IsOpen) 
        {
            window.DispatchEvents();
           
            window.Clear();
            switch (drawFilter)
            {
                case 0:
                window.Draw(spriteFilter);
                    break;
                case 1:
                window.Draw(spriteGrayScale);
                    break;
                case 2:
                    window.Draw(spriteNoFilter);
                    break;
            }
            window.Display();
        }
    }

}