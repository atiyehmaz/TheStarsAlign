using System.Text.RegularExpressions;

namespace TheStarsAlign
{
    public class navigator
    {
        public string GetMessage()
        {
            int height = 10;
            string output;

            var input = new PuzzleInput().getInput();

            var regex = new Regex(@"position=\<\s*(?<x>-?\d+),\s*(?<y>-?\d+)\> velocity=\<\s*(?<vx>-?\d+),\s*(?<vy>-?\d+)\>");

            var points = input.Split("\n").Select(line =>
            {
                var groups = regex.Match(line);
                return new Point
                {
                    PositionX = int.Parse(groups.Groups["x"].Value),
                    PositionY = int.Parse(groups.Groups["y"].Value),
                    VelocityX = int.Parse(groups.Groups["vx"].Value),
                    VelocityY = int.Parse(groups.Groups["vy"].Value)
                };
            }).ToList();


            while (true)
            {
                foreach (var point in points)
                {
                    point.PositionX += point.VelocityX;
                    point.PositionY += point.VelocityY;
                }

                var minimumX = points.Min(x => x.PositionX);
                var minimumY = points.Min(x => x.PositionY);
                var maximumX = points.Max(x => x.PositionX);
                var maximumY = points.Max(x => x.PositionY);

                if (!(maximumY - minimumY < height))
                    continue;

                var hashset = new HashSet<(int x, int y)>();

                points.Select(point => hashset.Add((point.PositionX, point.PositionY))).ToList();

                var result = new List<List<char>>();
                for (var i = 0; i <= maximumY - minimumY; i++)
                {
                    var charList = new List<char>();
                    for (var j = 0; j <= maximumX - minimumX; j++)
                        charList.Add('.');

                    result.Add(charList);
                }

                for (var x = minimumX; x <= maximumX; x++)
                {
                    for (var y = minimumY; y <= maximumY; y++)
                        result[y - minimumY][x - minimumX] = hashset.Contains((x, y)) ? '#' : '.';
                }

                output = (Environment.NewLine +
                string.Join(Environment.NewLine, result.Select(c => string.Join(string.Empty, c))));

                return output;
            }
        }
    }
}
