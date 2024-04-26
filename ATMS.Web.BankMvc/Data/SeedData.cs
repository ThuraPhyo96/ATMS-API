using ATMS.Web.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace ATMS.Web.BankMvc.Data
{
    public class SeedData
    {
        public static async Task Initialize(ApplicationDBContext context)
        {
            if (context.Regions.Any())
                return;

            await CreateRegion(context);
            await CreateDivision(context);
            await CreateTownship(context);
        }

        private static async Task CreateRegion(ApplicationDBContext context)
        {
            List<Region> regions =
            [
                new("Ayeyarwady Region", "", 1),
                new("Bago Region", "", 2),
                new("Chin State", "", 3),
                new("Kachin State", "", 4),
                new("Kayah State", "", 5),

                new("Kayin State", "", 6),
                new("Magway Region", "", 7),
                new("Mandalay Region", "", 8),
                new("Mon State", "", 9),
                new("Naypyidaw Union Territory", "", 15),

                new("Rakhine State", "", 10),
                new("Sagaing Region", "", 11),
                new("Shan State", "", 12),
                new("Tanintharyi Region", "", 13),
                new("Yangon Region", "", 14)
            ];

            await context.Regions.AddRangeAsync(regions);
            await context.SaveChangesAsync();
        }

        private static async Task CreateDivision(ApplicationDBContext context)
        {
            var regionObjs = await context.Regions.ToListAsync();

            Region? ayaRegion = regionObjs.FirstOrDefault(x => x.Name == "Ayeyarwady Region");
            Region? bagoRegion = regionObjs.FirstOrDefault(x => x.Name == "Bago Region");
            Region? yangonRegion = regionObjs.FirstOrDefault(x => x.Name == "Yangon Region");

            List<Division> divisins =
            [
                // Ayeyarwady Divisions
                new(ayaRegion!.RegionId, "Hinthada", "", 1),
                new(ayaRegion!.RegionId, "Labutta", "", 2),
                new(ayaRegion!.RegionId, "Ma-ubin", "", 3),
                new(ayaRegion!.RegionId, "Myaungmya", "", 4),
                new(ayaRegion!.RegionId, "Pathein", "", 5),
                new(ayaRegion!.RegionId, "Pyapon", "", 6),

                // Bago Divisions
                new(bagoRegion!.RegionId, "Bago", "", 1),
                new(bagoRegion!.RegionId, "Taungoo", "", 2),
                new(bagoRegion!.RegionId, "Pyay", "", 3),
                new(bagoRegion!.RegionId, "Thayarwady", "", 4),

                // Yangon Divisions
                new(yangonRegion!.RegionId, "East Yangon", "", 1),
                new(yangonRegion!.RegionId, "North Yangon", "", 2),
                new(yangonRegion!.RegionId, "South Yangon", "", 3),
                new(yangonRegion!.RegionId, "West Yangon(Downtown)", "", 4)
            ];

            await context.Divisions.AddRangeAsync(divisins);
            await context.SaveChangesAsync();
        }

        private static async Task CreateTownship(ApplicationDBContext context)
        {
            var divisionObjs = await context.Divisions.ToListAsync();

            // Ayeyarwady Divisions
            Division? hinthadaDivision = divisionObjs.FirstOrDefault(x => x.Name == "Hinthada");
            Division? labuttaDivision = divisionObjs.FirstOrDefault(x => x.Name == "Labutta");
            Division? maubinDivision = divisionObjs.FirstOrDefault(x => x.Name == "Ma-ubin");
            Division? maungmyaDivision = divisionObjs.FirstOrDefault(x => x.Name == "Myaungmya");
            Division? patheinDivision = divisionObjs.FirstOrDefault(x => x.Name == "Pathein");
            Division? pyaponDivision = divisionObjs.FirstOrDefault(x => x.Name == "Pyapon");

            // Bago Divisions
            Division? bagoDivision = divisionObjs.FirstOrDefault(x => x.Name == "Bago");
            Division? taungooDivision = divisionObjs.FirstOrDefault(x => x.Name == "Taungoo");
            Division? pyayDivision = divisionObjs.FirstOrDefault(x => x.Name == "Pyay");
            Division? thayarwadyDivision = divisionObjs.FirstOrDefault(x => x.Name == "Thayarwady");

            // Yangon Divisions
            Division? eastYgnDivision = divisionObjs.FirstOrDefault(x => x.Name == "East Yangon");
            Division? northYgnDivision = divisionObjs.FirstOrDefault(x => x.Name == "North Yangon");
            Division? southYgnDivision = divisionObjs.FirstOrDefault(x => x.Name == "South Yangon");
            Division? westYgnDivision = divisionObjs.FirstOrDefault(x => x.Name == "West Yangon(Downtown)");

            List<Township> ayaTownships =
                [
                    // Hinthada townships
                    new(hinthadaDivision!.RegionId, hinthadaDivision!.DivisionId, "Hinthada", "", 1),
                    new(hinthadaDivision!.RegionId, hinthadaDivision!.DivisionId, "Ingapu", "", 2),
                    new(hinthadaDivision!.RegionId, hinthadaDivision!.DivisionId, "Kyangin", "", 3),
                    new(hinthadaDivision!.RegionId, hinthadaDivision!.DivisionId, "Lemyethna", "", 4),
                    new(hinthadaDivision!.RegionId, hinthadaDivision!.DivisionId, "Myanaung", "", 5),
                    new(hinthadaDivision!.RegionId, hinthadaDivision!.DivisionId, "Zalun", "", 6),

                    // Labutta townships
                    new(labuttaDivision!.RegionId, labuttaDivision!.DivisionId, "Labutta", "", 1),
                    new(labuttaDivision!.RegionId, labuttaDivision!.DivisionId, "Mawlamyinegyun", "", 2),
                    new(labuttaDivision!.RegionId, labuttaDivision!.DivisionId, "Pyinsalu", "", 3),

                    // Ma-ubin
                    new(maubinDivision!.RegionId, maubinDivision!.DivisionId, "Danuphyu", "", 1),
                    new(maubinDivision!.RegionId, maubinDivision!.DivisionId, "Ma-ubin", "", 2),
                    new(maubinDivision!.RegionId, maubinDivision!.DivisionId, "Nyaungdon", "", 3),
                    new(maubinDivision!.RegionId, maubinDivision!.DivisionId, "Pantanaw", "", 4),

                    // Myaungmya
                    new(maungmyaDivision!.RegionId, maungmyaDivision!.DivisionId, "Einme", "", 1),
                    new(maungmyaDivision!.RegionId, maungmyaDivision!.DivisionId, "Myaungmya", "", 2),
                    new(maungmyaDivision!.RegionId, maungmyaDivision!.DivisionId, "Wakema", "", 3),

                    // Pathein
                    new(patheinDivision!.RegionId, patheinDivision!.DivisionId, "Hainggyikyun", "", 1),
                    new(patheinDivision!.RegionId, patheinDivision!.DivisionId, "Kangyidaunk", "", 2),
                    new(patheinDivision!.RegionId, patheinDivision!.DivisionId, "Kyaunggon", "", 3),
                    new(patheinDivision!.RegionId, patheinDivision!.DivisionId, "Kyonpyaw", "", 4),
                    new(patheinDivision!.RegionId, patheinDivision!.DivisionId, "Ngapudaw", "", 5),

                    new(patheinDivision!.RegionId, patheinDivision!.DivisionId, "Ngathaingchaung", "", 6),
                    new(patheinDivision!.RegionId, patheinDivision!.DivisionId, "Ngayokaung", "", 7),
                    new(patheinDivision!.RegionId, patheinDivision!.DivisionId, "Ngwehsaung", "", 8),
                    new(patheinDivision!.RegionId, patheinDivision!.DivisionId, "Pathein", "", 9),
                    new(patheinDivision!.RegionId, patheinDivision!.DivisionId, "Shwethaungyan", "", 10),

                    new(patheinDivision!.RegionId, patheinDivision!.DivisionId, "Thabaung", "", 11),
                    new(patheinDivision!.RegionId, patheinDivision!.DivisionId, "Yekyi", "", 12),

                    // Pyapon
                    new(pyaponDivision!.RegionId, pyaponDivision!.DivisionId, "Ahmar", "", 1),
                    new(pyaponDivision!.RegionId, pyaponDivision!.DivisionId, "Bogale", "", 2),
                    new(pyaponDivision!.RegionId, pyaponDivision!.DivisionId, "Dedaye", "", 3),
                    new(pyaponDivision!.RegionId, pyaponDivision!.DivisionId, "Kyaiklat", "", 4),
                    new(pyaponDivision!.RegionId, pyaponDivision!.DivisionId, "Pyapon", "", 5)
                ];

            List<Township> bgoTownships =
            [
                // Bago townships
                new(bagoDivision!.RegionId, bagoDivision!.DivisionId, "Aungmyin", "", 1),
                new(bagoDivision!.RegionId, bagoDivision!.DivisionId, "Bago", "", 2),
                new(bagoDivision!.RegionId, bagoDivision!.DivisionId, "Daik-U", "", 3),
                new(bagoDivision!.RegionId, bagoDivision!.DivisionId, "Hpayargyi", "", 4),
                new(bagoDivision!.RegionId, bagoDivision!.DivisionId, "Intagaw", "", 5),

                new(bagoDivision!.RegionId, bagoDivision!.DivisionId, "Kawa", "", 6),
                new(bagoDivision!.RegionId, bagoDivision!.DivisionId, "Kyauktaga", "", 7),
                new(bagoDivision!.RegionId, bagoDivision!.DivisionId, "Madauk", "", 8),
                new(bagoDivision!.RegionId, bagoDivision!.DivisionId, "Nyaunglebin", "", 9),
                new(bagoDivision!.RegionId, bagoDivision!.DivisionId, "Peinzalot", "", 10),

                new(bagoDivision!.RegionId, bagoDivision!.DivisionId, "Penwegon", "", 11),
                new(bagoDivision!.RegionId, bagoDivision!.DivisionId, "Pyuntaza", "", 12),
                new(bagoDivision!.RegionId, bagoDivision!.DivisionId, "Shwegyin", "", 13),
                new(bagoDivision!.RegionId, bagoDivision!.DivisionId, "Thanatpin", "", 14),
                new(bagoDivision!.RegionId, bagoDivision!.DivisionId, "Waw", "", 15),

                // Taungoo townships
                new(taungooDivision!.RegionId, taungooDivision!.DivisionId, "Kanyutkwin", "", 1),
                new(taungooDivision!.RegionId, taungooDivision!.DivisionId, "Kaytumadi", "", 2),
                new(taungooDivision!.RegionId, taungooDivision!.DivisionId, "Kyaukkyi", "", 3),
                new(taungooDivision!.RegionId, taungooDivision!.DivisionId, "Kywebwe", "", 4),
                new(taungooDivision!.RegionId, taungooDivision!.DivisionId, "Mone", "", 5),

                new(taungooDivision!.RegionId, taungooDivision!.DivisionId, "Myohla", "", 6),
                new(taungooDivision!.RegionId, taungooDivision!.DivisionId, "Natthangwin", "", 7),
                new(taungooDivision!.RegionId, taungooDivision!.DivisionId, "Nyaungbinthar", "", 8),
                new(taungooDivision!.RegionId, taungooDivision!.DivisionId, "Oktwin", "", 9),
                new(taungooDivision!.RegionId, taungooDivision!.DivisionId, "Pyu", "", 10),

                new(taungooDivision!.RegionId, taungooDivision!.DivisionId, "Swa", "", 11),
                new(taungooDivision!.RegionId, taungooDivision!.DivisionId, "Tantabin", "", 12),
                new(taungooDivision!.RegionId, taungooDivision!.DivisionId, "Taungoo", "", 13),
                new(taungooDivision!.RegionId, taungooDivision!.DivisionId, "Thagara", "", 14),
                new(taungooDivision!.RegionId, taungooDivision!.DivisionId, "Yaeni", "", 15),
                new(taungooDivision!.RegionId, taungooDivision!.DivisionId, "Yedashe", "", 16),

                // Pyay townships
                new(pyayDivision!.RegionId, pyayDivision!.DivisionId, "Innma", "", 1),
                new(pyayDivision!.RegionId, pyayDivision!.DivisionId, "Okshipin", "", 2),
                new(pyayDivision!.RegionId, pyayDivision!.DivisionId, "Padaung", "", 3),
                new(pyayDivision!.RegionId, pyayDivision!.DivisionId, "Padigone", "", 4),
                new(pyayDivision!.RegionId, pyayDivision!.DivisionId, "Paukkaung", "", 5),

                new(pyayDivision!.RegionId, pyayDivision!.DivisionId, "Paungdale", "", 6),
                new(pyayDivision!.RegionId, pyayDivision!.DivisionId, "Paungde", "", 7),
                new(pyayDivision!.RegionId, pyayDivision!.DivisionId, "Pyay", "", 1),
                new(pyayDivision!.RegionId, pyayDivision!.DivisionId, "Shwedaung", "", 8),
                new(pyayDivision!.RegionId, pyayDivision!.DivisionId, "Sinmeswe", "", 9),
                new(pyayDivision!.RegionId, pyayDivision!.DivisionId, "Thegon", "", 10),

                // Thayarwady townships
                new(thayarwadyDivision!.RegionId, thayarwadyDivision!.DivisionId, "Gyobingauk", "", 1),
                new(thayarwadyDivision!.RegionId, thayarwadyDivision!.DivisionId, "Letpadan", "", 2),
                new(thayarwadyDivision!.RegionId, thayarwadyDivision!.DivisionId, "Minhla", "", 3),
                new(thayarwadyDivision!.RegionId, thayarwadyDivision!.DivisionId, "Monyo", "", 4),
                new(thayarwadyDivision!.RegionId, thayarwadyDivision!.DivisionId, "Nattalin", "", 5),

                new(thayarwadyDivision!.RegionId, thayarwadyDivision!.DivisionId, "Okpho", "", 6),
                new(thayarwadyDivision!.RegionId, thayarwadyDivision!.DivisionId, "Ooethegone", "", 7),
                new(thayarwadyDivision!.RegionId, thayarwadyDivision!.DivisionId, "Sitkwin", "", 8),
                new(thayarwadyDivision!.RegionId, thayarwadyDivision!.DivisionId, "Tapun", "", 9),
                new(thayarwadyDivision!.RegionId, thayarwadyDivision!.DivisionId, "Tharrawaddy", "", 10),

                new(thayarwadyDivision!.RegionId, thayarwadyDivision!.DivisionId, "Thonze", "", 11),
                new(thayarwadyDivision!.RegionId, thayarwadyDivision!.DivisionId, "Zigon", "", 12)
            ];

            List<Township> ygnTownships =
                [
                // East Yangon townships
                new(eastYgnDivision!.RegionId, eastYgnDivision!.DivisionId, "Botataung", "", 1),
                    new(eastYgnDivision!.RegionId, eastYgnDivision!.DivisionId, "East Yangon City", "", 2),
                    new(eastYgnDivision!.RegionId, eastYgnDivision!.DivisionId, "Dagon Seikkan", "", 3),
                    new(eastYgnDivision!.RegionId, eastYgnDivision!.DivisionId, "Dawbon", "", 4),
                    new(eastYgnDivision!.RegionId, eastYgnDivision!.DivisionId, "East Dagon", "", 5),

                    new(eastYgnDivision!.RegionId, eastYgnDivision!.DivisionId, "Mingala Taungnyunt", "", 6),
                    new(eastYgnDivision!.RegionId, eastYgnDivision!.DivisionId, "North Dagon", "", 7),
                    new(eastYgnDivision!.RegionId, eastYgnDivision!.DivisionId, "North Okkalapa", "", 8),
                    new(eastYgnDivision!.RegionId, eastYgnDivision!.DivisionId, "Pazundaung", "", 9),
                    new(eastYgnDivision!.RegionId, eastYgnDivision!.DivisionId, "South Dagon", "", 10),

                    new(eastYgnDivision!.RegionId, eastYgnDivision!.DivisionId, "South Okkalapa", "", 11),
                    new(eastYgnDivision!.RegionId, eastYgnDivision!.DivisionId, "Tamwe", "", 12),
                    new(eastYgnDivision!.RegionId, eastYgnDivision!.DivisionId, "Thaketa", "", 13),
                    new(eastYgnDivision!.RegionId, eastYgnDivision!.DivisionId, "Thingangyun", "", 41),
                    new(eastYgnDivision!.RegionId, eastYgnDivision!.DivisionId, "Yankin", "", 15),

                    // North Yangon townships
                    new(northYgnDivision!.RegionId, northYgnDivision!.DivisionId, "North Yangon City", "", 1),
                    new(northYgnDivision!.RegionId, northYgnDivision!.DivisionId, "Hlaingthaya", "", 2),
                    new(northYgnDivision!.RegionId, northYgnDivision!.DivisionId, "Hlegu", "", 3),
                    new(northYgnDivision!.RegionId, northYgnDivision!.DivisionId, "Hmawbi", "", 4),
                    new(northYgnDivision!.RegionId, northYgnDivision!.DivisionId, "Htantabin", "", 5),

                    new(northYgnDivision!.RegionId, northYgnDivision!.DivisionId, "Insein", "", 6),
                    new(northYgnDivision!.RegionId, northYgnDivision!.DivisionId, "Mingaladon", "", 7),
                    new(northYgnDivision!.RegionId, northYgnDivision!.DivisionId, "Rural", "", 8),
                    new(northYgnDivision!.RegionId, northYgnDivision!.DivisionId, "Shwepyitha", "", 9),
                    new(northYgnDivision!.RegionId, northYgnDivision!.DivisionId, "Taikkyi", "", 10),

                    //  South Yangon
                    new(southYgnDivision!.RegionId, southYgnDivision!.DivisionId, "South Yangon City", "", 1),
                    new(southYgnDivision!.RegionId, southYgnDivision!.DivisionId, "Cocokyun", "", 2),
                    new(southYgnDivision!.RegionId, southYgnDivision!.DivisionId, "Dala", "", 3),
                    new(southYgnDivision!.RegionId, southYgnDivision!.DivisionId, "Kawhmu", "", 4),
                    new(southYgnDivision!.RegionId, southYgnDivision!.DivisionId, "Kayan", "", 5),

                    new(southYgnDivision!.RegionId, southYgnDivision!.DivisionId, "Kungyangon", "", 6),
                    new(southYgnDivision!.RegionId, southYgnDivision!.DivisionId, "Kyauktan", "", 7),
                    new(southYgnDivision!.RegionId, southYgnDivision!.DivisionId, "Rural", "", 8),
                    new(southYgnDivision!.RegionId, southYgnDivision!.DivisionId, "Seikkyi Kanaungto", "", 9),
                    new(southYgnDivision!.RegionId, southYgnDivision!.DivisionId, "Tada", "", 10),

                    new(southYgnDivision!.RegionId, southYgnDivision!.DivisionId, "Thanlyin", "", 11),
                    new(southYgnDivision!.RegionId, southYgnDivision!.DivisionId, "Thongwa", "", 12),
                    new(southYgnDivision!.RegionId, southYgnDivision!.DivisionId, "Twante", "", 13),

                    // West Yangon(Downtown)
                    new(westYgnDivision!.RegionId, westYgnDivision!.DivisionId, "Ahlon", "", 1),
                    new(westYgnDivision!.RegionId, westYgnDivision!.DivisionId, "Bahan", "", 2),
                    new(westYgnDivision!.RegionId, westYgnDivision!.DivisionId, "West Yangon(Downtown) City", "", 3),
                    new(westYgnDivision!.RegionId, westYgnDivision!.DivisionId, "Dagon", "", 4),
                    new(westYgnDivision!.RegionId, westYgnDivision!.DivisionId, "Hlaing", "", 5),

                    new(westYgnDivision!.RegionId, westYgnDivision!.DivisionId, "Kamayut", "", 6),
                    new(westYgnDivision!.RegionId, westYgnDivision!.DivisionId, "Kyauktada", "", 7),
                    new(westYgnDivision!.RegionId, westYgnDivision!.DivisionId, "Kyimyindaing", "", 8),
                    new(westYgnDivision!.RegionId, westYgnDivision!.DivisionId, "Lanmadaw", "", 9),
                    new(westYgnDivision!.RegionId, westYgnDivision!.DivisionId, "Latha", "", 10),

                    new(westYgnDivision!.RegionId, westYgnDivision!.DivisionId, "Mayangon", "", 11),
                    new(westYgnDivision!.RegionId, westYgnDivision!.DivisionId, "Pabedan", "", 12),
                    new(westYgnDivision!.RegionId, westYgnDivision!.DivisionId, "Sanchaung", "", 13),
                    new(westYgnDivision!.RegionId, westYgnDivision!.DivisionId, "Seikkan", "", 14)
                ];

            await context.Townships.AddRangeAsync(ayaTownships);
            await context.Townships.AddRangeAsync(bgoTownships);
            await context.Townships.AddRangeAsync(ygnTownships);

            await context.SaveChangesAsync();
        }
    }
}
