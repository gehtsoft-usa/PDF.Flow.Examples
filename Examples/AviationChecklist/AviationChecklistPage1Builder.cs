using AviationChecklist.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Newtonsoft.Json;
using System.IO;
using static AviationChecklist.AviationChecklistBuilder;

namespace AviationChecklist
{
    internal static class AviationChecklistPage1Builder
    {
        internal static void Build(SectionBuilder section)
        {
            var mainTable = CreateMainTable(section);
            var mainRow = mainTable.AddRow();

            CreateLeftColumn(mainRow.AddCell());
            mainRow.AddCell();
            CreateRightColumn(mainRow.AddCell());
        }

        internal static void CreateLeftColumn(TableCellBuilder leftCell)
        {
            var leftColumnTable = leftCell.AddTable();
            leftColumnTable
                .AddColumnPercentToTable("", 100)
                .SetBorderStroke(Stroke.None);

            CreateSectionHeader(leftColumnTable, "PRE FLIGHT FLOWS");
            var cellPreFlightFlows = CreateSectionCell(leftColumnTable);
            cellPreFlightFlows
                .SetPadding(1, 4, 0, 0)
                .AddParagraphToCell("Download Charts @ NOTAMS")
                .AddParagraphToCell("Check weather & forecasts")
                    .AddParagraph("Load PAX, CARGO & FUEL")
                    .SetLineSpacing(0.8f);

            CreateEmptyCell(leftColumnTable);
            CreatePreStartChecklist(leftColumnTable);
            CreateStartupChecklist(leftColumnTable);
        }

        private static void CreatePreStartChecklist(TableBuilder leftColumnTable)
        {
            PreStartData preStartData =
                JsonConvert.DeserializeObject<PreStartData>
                    (File.ReadAllText(Path.Combine("Content", "pre-start.json")));

            CreateSectionHeader(leftColumnTable, preStartData.Name);

            CreateChecklistItem(leftColumnTable, "PARKING BRAKE", 
                preStartData.ParkingBrake, true);
            CreateChecklistItem(leftColumnTable, "CHOCKS", preStartData.Chocks);
            CreateChecklistItem(leftColumnTable, "GPU", preStartData.GPU);
            CreateChecklistItem(leftColumnTable, "THROTTLE", preStartData.Throttle);
            CreateChecklistItem(leftColumnTable, "ENGINE MASTERS", preStartData.EngineMasters);
            CreateChecklistItem(leftColumnTable, "BATTERIES", preStartData.Batteries);
            CreateChecklistItem(leftColumnTable, "GENERATOR SWITCHES", 
                preStartData.GeneratorSwitches);
            CreateChecklistItem(leftColumnTable, "EXT POWER", preStartData.ExtPower);
            CreateChecklistItem(leftColumnTable, "ADIRS", preStartData.Adirs);
            CreateChecklistItem(leftColumnTable, "PANEL DISPLAYS", preStartData.PanelDisplay);
            CreateChecklistItem(leftColumnTable, "NAV LIGHTS", preStartData.NavLights);
            CreateChecklistItem(leftColumnTable, "PANEL LIGHTS", preStartData.PanelLights);
            CreateChecklistItem(leftColumnTable, "LANDING GEAR LEVER", 
                preStartData.LandingGearLever);
            CreateChecklistItem(leftColumnTable, "FLAPS", preStartData.Flaps);
            CreateChecklistItem(leftColumnTable, "SPOILER", preStartData.Spoiler);
            CreateChecklistItem(leftColumnTable, "FUEL QUANTITY", preStartData.FuelQuantity);
            CreateChecklistItem(leftColumnTable, "FASTEN SEAT BELTS",
                preStartData.FastenSeatBelts);
            CreateChecklistItem(leftColumnTable, "NO SMOKING SIGNS", 
                preStartData.NoSmokingSigns);
            CreateBoldParagraph(leftColumnTable, "Check Weather (ATIS, Flight Services)");
            CreateChecklistItem(leftColumnTable, "DE-ICE", preStartData.DeIce);
            CreateBoldParagraph(leftColumnTable, "Request Clearance");
            CreateChecklistItem(leftColumnTable, "TRANSPONDER", preStartData.Transponder);
            CreateChecklistItem(leftColumnTable, "BEACON LIGHTS", preStartData.BeaconLights);
            CreateChecklistItem(leftColumnTable, "EMERGENCY LIGHTS", 
                preStartData.EmergencyLights);
            CreateChecklistItem(leftColumnTable, "FMC", preStartData.FMC);
            CreateChecklistItem(leftColumnTable, "DEPARTURE BRIEFING",
                preStartData.DepartureBriefing);
            CreateChecklistItem(leftColumnTable, "DOORS", preStartData.Doors, false, true);
            CreateEmptyCell(leftColumnTable);
        }

        private static void CreateStartupChecklist(TableBuilder leftColumnTable)
        {
            StartupData startupData =
                JsonConvert.DeserializeObject<StartupData>
                    (File.ReadAllText(Path.Combine("Content", "startup.json")));

            CreateSectionHeader(leftColumnTable, startupData.Name);

            CreateChecklistItem(leftColumnTable, "APU", startupData.APU1, true);
            CreateChecklistItem(leftColumnTable, "APU BLEED", startupData.APUBleed);
            CreateChecklistItem(leftColumnTable, "APU GEN", startupData.APUGen);
            CreateBoldParagraph(leftColumnTable, "Request Pushback – Initiate Pushback");
            CreateChecklistItem(leftColumnTable, "THRUST LEVERS", startupData.ThrustLevers);
            CreateChecklistItem(leftColumnTable, "ENGINE AREA", startupData.EngineArea);
            CreateChecklistItem(leftColumnTable, "FUEL PUMP SWITCHES", 
                startupData.FuelPumpSwitches);
            CreateChecklistItem(leftColumnTable, "MODE SELECTOR", startupData.ModeSelector1);
            CreateChecklistItem(leftColumnTable, "LEFT ENGINE", startupData.LeftEngine);
            CreateChecklistItem(leftColumnTable, "ENGINE MASTER 1", startupData.EngineMaster1);
            CreateChecklistItem(leftColumnTable, "AT N2 > 20% FUEL FLOW", 
                startupData.AtHighN2FuelFlow);
            CreateChecklistItem(leftColumnTable, "N1 INCREASING AS N2 INCR.", 
                startupData.N1IncreaseAsN2Increase);
            CreateChecklistItem(leftColumnTable, "OIL PRESSURE", startupData.OilPressure);
            CreateChecklistItem(leftColumnTable, "GENERATOR SWITCH", startupData.GeneratorSwitch);
            CreateChecklistItem(leftColumnTable, "REPEAT FOR RIGHT ENGINE",
                startupData.RepeatForRightEngine);
            CreateChecklistItem(leftColumnTable, "FUEL FLOW", startupData.FuelFlow);
            CreateChecklistItem(leftColumnTable, "HYDRAULIC PUMP SWITCHES",
                startupData.HydraulicPumpSwitches);
            CreateChecklistItem(leftColumnTable, "APU", startupData.APU2);
            CreateChecklistItem(leftColumnTable, "MODE SELECTOR", 
                startupData.ModeSelector2, false, true);
        }

        private static void CreateRightColumn(TableCellBuilder rightCell)
        {
            var rightColumnTable = rightCell.AddTable();
            rightColumnTable
                .AddColumnPercentToTable("", 100)
                .SetBorderStroke(Stroke.None);

            CreateBeforeTaxiChecklist(rightColumnTable);
            CreateTaxiChecklist(rightColumnTable);
            CreateBeforeTakeOffChecklist(rightColumnTable);
            CreateTakeOffChecklist(rightColumnTable);
            CreateClimbOutChecklist(rightColumnTable);
        }

        private static void CreateBeforeTaxiChecklist(TableBuilder rightColumnTable)
        {
            BeforeTaxiData beforeTaxiData =
                JsonConvert.DeserializeObject<BeforeTaxiData>
                    (File.ReadAllText(Path.Combine("Content", "before-taxi.json")));

            CreateSectionHeader(rightColumnTable, beforeTaxiData.Name);

            CreateChecklistItem(rightColumnTable, "PROBE/WINDOW HEAT",
                beforeTaxiData.ProbeWindowHeat, true);
            CreateChecklistItem(rightColumnTable, "HDG INDICATOR / ALTIMITERS",
                beforeTaxiData.HDGIndicatorAltimeters);
            CreateChecklistItem(rightColumnTable, "STBY INSTRUMENTS", 
                beforeTaxiData.STDByInstruments);
            CreateChecklistItem(rightColumnTable, "RADIOS AND AVIONICS",
                beforeTaxiData.RadiosAndAvionics);
            CreateChecklistItem(rightColumnTable, "AUTOPILOT", beforeTaxiData.Autopilot);
            CreateChecklistItem(rightColumnTable, "F/D", beforeTaxiData.FD);
            CreateChecklistItem(rightColumnTable, "AUTOBRAKE", beforeTaxiData.Autobrake);
            CreateChecklistItem(rightColumnTable, "ELEVATOR TRIM", 
                beforeTaxiData.ElevatorTrim);
            CreateChecklistItem(rightColumnTable, "FLIGHT CONTROLS", 
                beforeTaxiData.FlightControls);
            CreateBoldParagraph(rightColumnTable, "Request Taxi Clearance", false, true);
            CreateEmptyCell(rightColumnTable);
        }

        private static void CreateTaxiChecklist(TableBuilder rightColumnTable)
        {
            TaxiData taxiData =
                JsonConvert.DeserializeObject<TaxiData>
                    (File.ReadAllText(Path.Combine("Content", "taxi.json")));

            CreateSectionHeader(rightColumnTable, taxiData.Name);

            CreateChecklistItem(rightColumnTable, "TAXI LIGHTS", taxiData.TaxiLights, true);
            CreateChecklistItem(rightColumnTable, "PARKING BRAKE", taxiData.ParkingBrake);
            CreateChecklistItem(rightColumnTable, "TAXI to assigned runway", 
                taxiData.TaxiToAssignedRunway);
            CreateChecklistItem(rightColumnTable, "BRKS/GYRO/TURN COORDINATOR", 
                taxiData.BreaksGyroTurnCoordinator);
            CreateChecklistItem(rightColumnTable, "T/O CONFIG", taxiData.TOConfig);
            CreateChecklistItem(rightColumnTable, "T/O MEMO", taxiData.TOMemo, false, true);
            CreateEmptyCell(rightColumnTable);
        }

        private static void CreateBeforeTakeOffChecklist(TableBuilder rightColumnTable)
        {
            BeforeTakeOffData beforeTakeOffData =
                JsonConvert.DeserializeObject<BeforeTakeOffData>
                    (File.ReadAllText(Path.Combine("Content", "before-takeoff.json")));

            CreateSectionHeader(rightColumnTable, beforeTakeOffData.Name);

            CreateChecklistItem(rightColumnTable, "PARKING BRAKE",
                beforeTakeOffData.ParkingBrake, true);
            CreateChecklistItem(rightColumnTable, "FLIGHT INSTRUMENTS", 
                beforeTakeOffData.FlightInstruments);
            CreateChecklistItem(rightColumnTable, "ENGINE INSTRUMENTS", 
                beforeTakeOffData.EngineInstruments);
            CreateChecklistItem(rightColumnTable, "TAKE-OFF DATA", 
                beforeTakeOffData.TakeOffData);
            CreateChecklistItem(rightColumnTable, "NAV EQUIPMENT", 
                beforeTakeOffData.NavEquipment);
            CreateChecklistItem(rightColumnTable, "LANDING LIGHTS", 
                beforeTakeOffData.LandingLights);
            CreateChecklistItem(rightColumnTable, "STROBE LIGHT", 
                beforeTakeOffData.StrobeLight);
            CreateChecklistItem(rightColumnTable, "PITOT HEAT", beforeTakeOffData.PitotHeat);
            CreateChecklistItem(rightColumnTable, "DE-ICE", beforeTakeOffData.DeIce);
            CreateChecklistItem(rightColumnTable, "TRANSPONDER", 
                beforeTakeOffData.Transponder);
            CreateBoldParagraph(rightColumnTable, "Request Takeoff Clearance", false, true);
            CreateEmptyCell(rightColumnTable);
        }

        private static void CreateTakeOffChecklist(TableBuilder rightColumnTable)
        {
            TakeOffData takeOffData =
                JsonConvert.DeserializeObject<TakeOffData>
                    (File.ReadAllText(Path.Combine("Content", "takeoff.json")));

            CreateSectionHeader(rightColumnTable, takeOffData.Name);

            CreateBoldParagraph(rightColumnTable, 
                "Smoothly increase thrust to 40% N1 let spool up");
            CreateChecklistItem(rightColumnTable, "TAKEOFF THRUST", takeOffData.TakeoffThrust);
            CreateChecklistItem(rightColumnTable, "BRAKES", takeOffData.Brakes);
            CreateChecklistItem(rightColumnTable, "AT 100 KTS", takeOffData.At100KTS);
            CreateChecklistItem(rightColumnTable, "AT V1", takeOffData.AtV1);
            CreateChecklistItem(rightColumnTable, "AT Vr", takeOffData.AtVr);
            CreateChecklistItem(rightColumnTable, "PITCH", takeOffData.Pitch);
            CreateChecklistItem(rightColumnTable, "POSITIVE RATE OF CLIMB", 
                takeOffData.PositiveRateOfClimb);
            CreateChecklistItem(rightColumnTable, "PASSING F SPEED (PFD)", 
                takeOffData.PassingFSpeedPFD);
            CreateChecklistItem(rightColumnTable, "SPOILERS", takeOffData.Spoilers);
            CreateChecklistItem(rightColumnTable, "LANDING LIGHTS", 
                takeOffData.LandingLights, false, true);
            CreateEmptyCell(rightColumnTable);
        }

        private static void CreateClimbOutChecklist(TableBuilder rightColumnTable)
        {
            ClimbOutData climbOutData =
                JsonConvert.DeserializeObject<ClimbOutData>
                    (File.ReadAllText(Path.Combine("Content", "climbout.json")));

            CreateSectionHeader(rightColumnTable, climbOutData.Name);

            CreateChecklistItem(rightColumnTable, "THRUST LEVERS", 
                climbOutData.ThrustLevers, true);
            CreateChecklistItem(rightColumnTable, "AP1", climbOutData.AP1);
            CreateChecklistItem(rightColumnTable, "TAXI LIGHTS", climbOutData.TaxiLights);
            CreateBoldParagraph(rightColumnTable, "At TA (Transition-Altitude)");
            CreateChecklistItem(rightColumnTable, "ALTIMETER", climbOutData.Altimeter);
            CreateChecklistItem(rightColumnTable, "BELOW 10'000FT", climbOutData.Below10000Ft);
            CreateChecklistItem(rightColumnTable, "ATC", climbOutData.ATC);
            CreateBoldParagraph(rightColumnTable, "Passing 10'000 ft");
            CreateChecklistItem(rightColumnTable, "LANDING LIGHTS", climbOutData.LandingLights);
            CreateBoldParagraph(rightColumnTable, "Above 10'000 ft");
            CreateChecklistItem(rightColumnTable, "FASTEN SEAT BELTS", 
                climbOutData.FastenSeatBelts, false, true);
        }
    }
}