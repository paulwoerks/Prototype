namespace PocketHeroes.Characters
{
    public class JumpAbility : BaseAbility
    {
        public override void Perform(Hero hero)
        {
            hero.Animator.SetTrigger("Jump");
        }
    }
}
