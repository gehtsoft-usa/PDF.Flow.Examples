using AviationChecklist.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Newtonsoft.Json;
using System.IO;
using static AviationChecklist.AviationChecklistBuilder;

namespace AviationChecklist
{
    internal static class AviationChecklistPage2Builder
    {
        internal static void Build(SectionBuilder section)
        {
            section.InsertPageBreak();
            var mainTable = CreateMainTable(section);
            var mainRow = mainTable.AddRow();

            CreateLeftColumn(mainRow.AddCell());
            mainRow.AddCell();
            CreateRightColumn(mainRow.AddCell());
        }

        private static void CreateLeftColumn(TableCellBuilder leftCell)
        {
            var leftColumnTable = leftCell.AddTable();
            leftColumnTable
                .AddColumnPercentToTable("", 100)
                .SetBorderStroke(Stroke.None);

            CreateCruiseChecklist(leftColumnTable);
            CreateApproachChecklist(leftColumnTable);
            CreateLandingChecklist(leftColumnTable);
        }

        private static void CreateCruiseChecklist(TableBuilder leftColumnTable)
        {
            CruiseData cruiseData =
                JsonConvert.DeserializeObject<CruiseData>
                    (File.ReadAllText(Path.Combine("Content", "cruise.json")));

            CreateSectionHeader(leftColumnTable, cruiseData.Name);

            CreateBoldParagraph(leftColumnTable, "Accelerate to Cruise Speed", true);
            CreateChecklistItem(leftColumnTable, "ENGINE & INSTRUMENTS", 
                cruiseData.EngineAndInstruments);
            CreateChecklistItem(leftColumnTable, "FUEL QUANTITY", cruiseData.FuelQuantity);
            CreateChecklistItem(leftColumnTable, "RADIOS", cruiseData.Radios1);
            CreateChecklistItem(leftColumnTable, "AUTOPILOT", cruiseData.Autopilot);
            CreateChecklistItem(leftColumnTable, "LIGHTS", cruiseData.Lights);
            CreateChecklistItem(leftColumnTable, "ATIS / AIRPORT INFORMATION", 
                cruiseData.ATISAirportInformation);
            CreateChecklistItem(leftColumnTable, "ALTIMETER", cruiseData.Altimeter1);
            CreateChecklistItem(leftColumnTable, "RADIOS", cruiseData.Radios2);
            CreateChecklistItem(leftColumnTable, "DE-ICE", cruiseData.DeIce);
            CreateChecklistItem(leftColumnTable, "TOD", cruiseData.TOD);
            CreateChecklistItem(leftColumnTable, "DESCENT SPEED", cruiseData.DescentSpeed);
            CreateChecklistItem(leftColumnTable, "FL240", cruiseData.FL240);
            CreateChecklistItem(leftColumnTable, "FL180", cruiseData.FL180);
            CreateBoldParagraph(leftColumnTable, "At TA (Transition-Altitude)");
            CreateChecklistItem(leftColumnTable, "ALTIMETER", cruiseData.Altimeter2);
            CreateChecklistItem(leftColumnTable, "FL120", cruiseData.FL120);
            CreateBoldParagraph(leftColumnTable, "Below 10'000 ft");
            CreateChecklistItem(leftColumnTable, "SPEED", cruiseData.Speed);
            CreateChecklistItem(leftColumnTable, "LANDING LIGHTS", cruiseData.LandingLights);
            CreateChecklistItem(leftColumnTable, "LS", cruiseData.LS);
            CreateChecklistItem(leftColumnTable, "FUEL QUANTITIES & BALANCE", 
                cruiseData.FuelQuantitiesAndBalance);
            CreateChecklistItem(leftColumnTable, "FLAPS / LANDING GEAR", 
                cruiseData.FlapsLandingGear);
            CreateBoldParagraph(leftColumnTable, 
                "Check Weather (ATIS, Flight Services)", false, true);
            CreateEmptyCell(leftColumnTable);
        }

        private static void CreateApproachChecklist(TableBuilder leftColumnTable)
        {
            ApproachData approachData =
                JsonConvert.DeserializeObject<ApproachData>
                    (File.ReadAllText(Path.Combine("Content", "approach.json")));

            CreateSectionHeader(leftColumnTable, approachData.Name);

            CreateChecklistItem(leftColumnTable, "FASTEN SEAT BELTS", 
                approachData.FastenSeatBelts);
            CreateChecklistItem(leftColumnTable, "RADIOS", approachData.FastenSeatBelts);
            CreateChecklistItem(leftColumnTable, "SPEED", approachData.Speed1);
            CreateChecklistItem(leftColumnTable, "LANDING LIGHTS", approachData.LandingLights);
            CreateChecklistItem(leftColumnTable, "TAXI LIGHTS", approachData.TaxiLights);
            CreateChecklistItem(leftColumnTable, "GND SPOILERS", approachData.GNDSpoilers);
            CreateChecklistItem(leftColumnTable, "AUTO BRAKE", approachData.AutoBrake);
            CreateChecklistItem(leftColumnTable, "FLAPS", approachData.Flaps1);
            CreateChecklistItem(leftColumnTable, "SPEED", approachData.Speed2);
            CreateChecklistItem(leftColumnTable, "AT 6 DME", approachData.At6DME);
            CreateChecklistItem(leftColumnTable, "SPEED", approachData.Speed3);
            CreateChecklistItem(leftColumnTable, "LANDING GEAR", approachData.LandingGear);
            CreateChecklistItem(leftColumnTable, "LANDING GEAR 3 GREEN", 
                approachData.LandingGear3Green);
            CreateChecklistItem(leftColumnTable, "FLAPS", approachData.Flaps2);
            CreateBoldParagraph(leftColumnTable, "Final Glideslope Descent :");
            CreateChecklistItem(leftColumnTable, "SPEED", approachData.Speed4);
            CreateChecklistItem(leftColumnTable, "PARKING BRAKE", approachData.ParkingBrake);
            CreateChecklistItem(leftColumnTable, "DE-ICE", approachData.DeIce, false, true);
            CreateEmptyCell(leftColumnTable);
        }

        private static void CreateLandingChecklist(TableBuilder leftColumnTable)
        {
            LandingData landingData =
                JsonConvert.DeserializeObject<LandingData>
                    (File.ReadAllText(Path.Combine("Content", "landing.json")));

            CreateSectionHeader(leftColumnTable, landingData.Name);

            CreateChecklistItem(leftColumnTable, "LANDING GEAR", 
                landingData.LandingGear, true);
            CreateChecklistItem(leftColumnTable, "AUTOPILOT", landingData.Autopilot);
            CreateChecklistItem(leftColumnTable, "GO-AROUND ALTITUDE", 
                landingData.GoAroundAltitude);
            CreateChecklistItem(leftColumnTable, "AUTO-THRUST", landingData.AutoThrust);
            CreateChecklistItem(leftColumnTable, "LANDING MEMO", landingData.LandingMemo);
            CreateChecklistItem(leftColumnTable, "LANDING SPEED", landingData.LandingSpeed);
            CreateChecklistItem(leftColumnTable, "AFTER TOUCH DOWN", 
                landingData.AfterTouchDown);
            CreateChecklistItem(leftColumnTable, "SPOILERS", landingData.Spoilers);
            CreateChecklistItem(leftColumnTable, "BRAKES", landingData.Brakes);
            CreateChecklistItem(leftColumnTable, "AT 60 KIAS", 
                landingData.At60KIAS, false, true);
        }

        private static void CreateRightColumn(TableCellBuilder rightCell)
        {
            var rightColumnTable = rightCell.AddTable();
            rightColumnTable
                .AddColumnPercentToTable("", 100)
                .SetBorderStroke(Stroke.None);

            CreateAfterLandingChecklist(rightColumnTable);
            CreateParkingShutdownChecklist(rightColumnTable);
        }

        private static void CreateAfterLandingChecklist(TableBuilder rightColumnTable)
        {
            AfterLandingData afterLandingData =
                JsonConvert.DeserializeObject<AfterLandingData>
                    (File.ReadAllText(Path.Combine("Content", "after-landing.json")));

            CreateSectionHeader(rightColumnTable, afterLandingData.Name);

            CreateChecklistItem(rightColumnTable, "SPOILERS", afterLandingData.Spoilers, true);
            CreateChecklistItem(rightColumnTable, "FLAPS", afterLandingData.Flaps);
            CreateChecklistItem(rightColumnTable, "END MODE SELECTOR", 
                afterLandingData.EngModeSelector);
            CreateChecklistItem(rightColumnTable, "LANDING LIGHTS", 
                afterLandingData.LandingLights);
            CreateChecklistItem(rightColumnTable, "STROBE LIGHTS", 
                afterLandingData.StrobeLights);
            CreateChecklistItem(rightColumnTable, "ANTI ICE", afterLandingData.AntiIce);
            CreateChecklistItem(rightColumnTable, "APU", afterLandingData.APU1);
            CreateChecklistItem(rightColumnTable, "BRAKE TEMP", afterLandingData.BrakeTemp);
            CreateChecklistItem(rightColumnTable, "TRANSPONDER", afterLandingData.Transponder);
            CreateBoldParagraph(rightColumnTable, 
                "Taxi to Assigned Gate/Parking (Speed Max 20 knots)");
            CreateChecklistItem(rightColumnTable, "APU", afterLandingData.APU2);
            CreateChecklistItem(rightColumnTable, "APU", afterLandingData.APU3);
            CreateChecklistItem(rightColumnTable, "ELEVATOR TRIM", 
                afterLandingData.ElevatorTrim);
            CreateBoldParagraph(rightColumnTable, "Turning into the Gate:");
            CreateChecklistItem(rightColumnTable, "TAXI LIGHTS", 
                afterLandingData.TaxiLights, false, true);
            CreateEmptyCell(rightColumnTable);
        }

        private static void CreateParkingShutdownChecklist(TableBuilder rightColumnTable)
        {
            ParkingShutdownData parkingData =
                JsonConvert.DeserializeObject<ParkingShutdownData>
                    (File.ReadAllText(Path.Combine("Content", "parking-shutdown.json")));

            CreateSectionHeader(rightColumnTable, parkingData.Name);

            CreateChecklistItem(rightColumnTable, "PARKING BRAKES", 
                parkingData.ParkingBrakes1, true);
            CreateChecklistItem(rightColumnTable, "THRUST LEVERS", parkingData.ThrustLevers);
            CreateChecklistItem(rightColumnTable, "GROUND CONTACT", parkingData.GroundContact);
            CreateChecklistItem(rightColumnTable, "GROUND OPERATIONS", 
                parkingData.GroundOperations);
            CreateChecklistItem(rightColumnTable, "ELECTRICAL POWER", 
                parkingData.ElectricalPower);
            CreateChecklistItem(rightColumnTable, "ENGINE MASTER 1 & 2", 
                parkingData.EngineMaster1And2);
            CreateChecklistItem(rightColumnTable, "PARKING BRAKES", 
                parkingData.ParkingBrakes1);
            CreateChecklistItem(rightColumnTable, "NAV LIGHTS", parkingData.NavLights);
            CreateChecklistItem(rightColumnTable, "EXTERIOR LIGHTS", 
                parkingData.ExteriorLights);
            CreateChecklistItem(rightColumnTable, "ANTI ICE", parkingData.AntiIce);
            CreateChecklistItem(rightColumnTable, "PASSENGER SIGNS", 
                parkingData.PassengerSigns);
            CreateChecklistItem(rightColumnTable, "DOORS", parkingData.Doors);
            CreateChecklistItem(rightColumnTable, "FLIGHT DIRECTOR", 
                parkingData.FlightDirector);
            CreateChecklistItem(rightColumnTable, "APU BLEED", parkingData.APUBleed);
            CreateChecklistItem(rightColumnTable, "FUEL PUMPS", parkingData.FuelPumps);
            CreateChecklistItem(rightColumnTable, "BEACON", parkingData.Beacon);
            CreateChecklistItem(rightColumnTable, "ECAM STS", parkingData.ECAMSTS);
            CreateChecklistItem(rightColumnTable, "PANEL LIGHTS", parkingData.PanelLights);
            CreateChecklistItem(rightColumnTable, "ADIRS", parkingData.ADIRS);
            CreateChecklistItem(rightColumnTable, "AVIONICS", parkingData.Avionics);
            CreateChecklistItem(rightColumnTable, "NO SMOKING", parkingData.NoSmoking);
            CreateChecklistItem(rightColumnTable, "APU", parkingData.APU);
            CreateChecklistItem(rightColumnTable, "BATTERIES", 
                parkingData.Batteries, false, true);
        }
    }
}