using System;

namespace chainsharp.server
{
  public class WeatherForecast
  {
    #region Public Properties

    public DateTime Date { get; set; }

    public string Summary { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    #endregion Public Properties
  }
}
