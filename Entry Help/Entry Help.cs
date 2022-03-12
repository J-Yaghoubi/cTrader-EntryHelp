
////////////////////////////////////////////////////////////////////////////////////////
///                                      Entry Help                                  ///
///                      (Combination of RSI and SMA as Entry trigger)               ///
///                                                                                  ///
///         Publish date  13-MARCH-2022                                              ///
///         Version  1.0.0                                                           ///
///         By  Seyed Jafar Yaghoubi                                                 ///
///         License  MIT                                                             ///
///         More info https://github.com/J-Yaghoubi/                                 ///
///         Contact  algo3xp3rt@gmail.com                                            ///
///                                                                                  ///
////////////////////////////////////////////////////////////////////////////////////////

using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class EntryHelp : Indicator
    {
        #region parameters

        [Parameter("RSI Period", DefaultValue = 50, MinValue = 1, Group = "Preset")]
        public int rsiPeriod { get; set; }

        [Parameter("RSI Smoothness", DefaultValue = 2, MinValue = 1, Group = "Preset")]
        public int rsiSmoothness { get; set; }

        [Parameter("SMA Period", DefaultValue = 50, MinValue = 2, Group = "Preset")]
        public int smaPeriod { get; set; }

        [Output("RSI", LineColor = "FFBF9100", Thickness = 1)]
        public IndicatorDataSeries _RSIresult { get; set; }

        [Output("SMA", LineColor = "FF025776", Thickness = 1)]
        public IndicatorDataSeries _SMAresult { get; set; }

        #endregion

        #region Global variables

        private RelativeStrengthIndex RSI;
        private ExponentialMovingAverage EMA;
        private SimpleMovingAverage SMA;

        #endregion

        protected override void Initialize()
        {
            RSI = Indicators.RelativeStrengthIndex(Bars.ClosePrices, rsiPeriod);
            EMA = Indicators.ExponentialMovingAverage(RSI.Result, rsiSmoothness);
            SMA = Indicators.SimpleMovingAverage(EMA.Result, smaPeriod);
        }

        public override void Calculate(int index)
        {
            if (index < smaPeriod)
                return;

            _RSIresult[index] = EMA.Result.Last(0);
            _SMAresult[index] = SMA.Result.Last(0);

            ChartObjects.DrawText("Copyright", "ⒸS.J.Yaghoubi", StaticPosition.BottomLeft, Colors.Gray);

        }
    }
}
