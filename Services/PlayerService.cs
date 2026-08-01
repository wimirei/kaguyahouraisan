using Microsoft.EntityFrameworkCore;
using thrucommunity.Data;
using thrucommunity.Models;

namespace thrucommunity.Services
{
    public class PlayerService
    {
        private readonly ApplicationDbContext _context;


        public PlayerService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task UpdatePlayerStatistics(ReplayModel replay)
        {
            var player = await _context.Players
                .FirstOrDefaultAsync(x => x.Nickname == replay.Nickname);

            if (player == null)
            {
                player = new PlayerModel
                {
                    Nickname = replay.Nickname
                };

                _context.Players.Add(player);
            }


            if (replay.Difficulty == Difficulty.Lunatic)
            {
                // L1CC
                if (!replay.NoBomb &&
                    !replay.NoMiss &&
                    !replay.NoThirdCondition)
                {
                    player.L1CCcount++;
                    player.survivalpoints += 1;
                }

                // LNB
                if (replay.NoBomb &&
                    !replay.NoMiss &&
                    !replay.NoThirdCondition)
                {
                    player.LNBcount++;
                    player.survivalpoints += 5;
                }

                // LNM
                if (!replay.NoBomb &&
                    replay.NoMiss &&
                    !replay.NoThirdCondition)
                {
                    player.LNMcount++;
                    player.survivalpoints += 3;
                }

                // LNN
                if (replay.NoBomb &&
                    replay.NoMiss &&
                    !replay.NoThirdCondition)
                {
                    player.LNNcount++;
                    player.survivalpoints += 50;
                }

                // LNBNx
                if (replay.NoBomb &&
                    !replay.NoMiss &&
                    replay.NoThirdCondition)
                {
                    player.LNBNxcount++;
                    player.survivalpoints += 5;
                }

                //WBaWC LNBN

                if (replay.NoBomb &&
                    !replay.NoMiss &&
                    replay.No4thCondition)
                {
                    player.LNBNxcount++;
                    player.survivalpoints += 5;
                }


                // WBaWC LNBNN
                if (replay.NoBomb &&
                    !replay.NoMiss &&
                    replay.NoThirdCondition && replay.No4thCondition)
                {
                    player.LNBNxcount++;
                    player.survivalpoints += 5;
                }

                // LNNN
                if (replay.NoBomb &&
                    replay.NoMiss &&
                    replay.NoThirdCondition)
                {
                    player.LNNNcount++;
                    player.survivalpoints += 50;
                }

                // LNNNN
                if (replay.NoBomb &&
                    replay.NoMiss &&
                    replay.NoThirdCondition && replay.No4thCondition)
                {
                    player.LNNNcount++;
                    player.survivalpoints += 50;
                }


            }

        }
    }
}