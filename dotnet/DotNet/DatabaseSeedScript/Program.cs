// See https://aka.ms/new-console-template for more information
using DatabaseSeedScript;

Configure.SetupConfig();
var seedGen = new ExecuteSeed();
await seedGen.Go();

