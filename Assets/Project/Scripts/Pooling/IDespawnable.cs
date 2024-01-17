namespace PocketHeroes
{
    ///<summary>
    ///Pooled Objects can use this interface to call something when despawning
    ///</summary>///
    public interface IDespawnable { 
        void OnDespawn() { }
    }
}
