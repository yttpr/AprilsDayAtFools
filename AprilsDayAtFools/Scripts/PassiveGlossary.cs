using BrutalAPI;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class PassiveGlossary
    {
        public static void AddPassiveToGlossary(this BasePassiveAbilitySO self, string Name, string Description)
        {
            foreach (GlossaryPassives check in LoadedDBsHandler.GlossaryDB.Passives)
                if (check._name == Name) return;
            GlossaryPassives glossaryPassives = new GlossaryPassives(Name, Description, self.passiveIcon);
            LoadedDBsHandler.GlossaryDB.AddNewPassive(glossaryPassives);
        }
        public static void AddToPassiveDatabase(this BasePassiveAbilitySO self)
        {
            if (self.name == "" || self.name.Length <= 0) self.name = self.m_PassiveID;
            if (!LoadedDBsHandler.PassiveDB._PassivesPool.Contains(self.name))
                Passives.AddCustomPassiveToPool(self.name, self._passiveName, self);
        }
    }
}
