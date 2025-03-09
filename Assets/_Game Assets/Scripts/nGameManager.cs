public class nGameManager : MonoBehaviour
{
    public ScreenHandlers[] screenHandlers;

    private int score;
    private int health;
    private void Game()
    {
        var sceneLoadAsyncOperation = SceneManager.LoadSceneAsync("scene");
        if (sceneLoadAsyncOperation == null) return;

        sceneLoadAsyncOperation.allowDefaultTransition = false;
        yield return ShowScreen(ScreenType.STATUS);

        sceneLoadAsyncOperation.allowDefaultTransition = true;
    }

    public void OnMicrogameFinish(bool win)
    {
        yield return ShowScreen(win ? ScreenType.POSITIVE : ScreenType.NEGATIVE);
        if (win) score++;
        
        bool dead = UpdateHealth(win);
        if (dead)
        {
            ShowScreen(ScreenType.GAME_OVER);
            return;
        }

        Game();
    }

    private bool UpdateHealth(bool win)
    {
        health += win ? 0 : -1;
        return health <= 0;
    }
    
    private IEnumerator ShowScreen(ScreenType screenType)
    {
        screenHandlers[screenType].Show();
        yield return new WaitForSeconds(2f);
    }
}

public enum ScreenType
{
    STATUS,
    POSITIVE,
    NEGATIVE,
    GAME_OVER,
    BOSS,
    FASTER,
}