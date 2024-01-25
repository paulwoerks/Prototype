namespace PocketHeroes.Combat
{
    /// <summary>
    /// Every damagable object needs a IDamagable Component
    /// </summary>
    public interface IDamagable
    {
        void InflictDamage(int damage) { }
    }
}
