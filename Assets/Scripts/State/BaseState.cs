using TMPro;
using UnityEngine;

public class PlayingState : BaseState<MP_GameStateManager>
{
	public override void EnterState(MP_GameStateManager gameContext)
	{
		//Debug.Log($"{gameContext.transform.name} enter state {GetType().Name}");
		RoomManager.Instance.switchGameState.onClick.RemoveAllListeners();
		RoomManager.Instance.switchGameState.onClick.AddListener(() => { gameContext.SwitchState(gameContext.pauseState); });
		RoomManager.Instance.switchGameState.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{GetType().Name}";
		gameContext.enableUnits(true);


		//gameContext.SelectedUnit = gameContext.Units.FirstOrDefault();
		//UnitPhoton firstOneIdel = gameContext.EnemyUnits.FirstOrDefault(unit => unit.State is Idel_MP);
		//gameContext.SelectedUnit.CurrentTarget = firstOneIdel ?? gameContext.EnemyUnits.FirstOrDefault();




	}

	public override void ExitState(MP_GameStateManager gameContext)
	{
		//Debug.Log($"{gameContext.transform.name} EXIT state {GetType().Name}");
	}

	public override void Update(MP_GameStateManager gameContext)
	{
		//Debug.LogError($"{gameContext.transform.name} update  of {GetType().Name}");

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			//gameContext.SelectNextUnit();
		}

	}
}


public class PauseState : BaseState<MP_GameStateManager>
{
	public override void EnterState(MP_GameStateManager gameContext)
	{
		//Debug.Log($"{gameContext.transform.name} enter state {GetType().Name}");
		RoomManager.Instance.switchGameState.onClick.RemoveAllListeners();
		RoomManager.Instance.switchGameState.onClick.AddListener(() => { gameContext.SwitchState(gameContext.playingState); });
		RoomManager.Instance.switchGameState.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{GetType().Name}";

		gameContext.enableUnits(false);
	}

	public override void ExitState(MP_GameStateManager gameContext)
	{
		//Debug.Log($"{gameContext.transform.name} EXIT state {GetType().Name}");
	}

	public override void Update(MP_GameStateManager gameContext)
	{
		//Debug.LogError($"{gameContext.transform.name} update  of {GetType().Name}");
	}
}




public class PlayerTurn : BaseState<SP_GameStateManager>
{
	//public override AnyClass EnterState(SP_GameStateManager gameManager)
	//{
	//	gameManager.SelectedUnit = gameManager.players.FirstOrDefault();
	//	gameManager.SelectedUnit.CurrentTarget = gameManager.enemies.FirstOrDefault(unit => unit.State is Idel);

	//	gameManager.SelectedUnit.enabled = true;
	//	gameManager.SelectedUnit.fpsCam.enabled = true;
	//	gameManager.SelectedUnit.onCameraEnabeled();
	//	//gameManager.PlayerChangeEvent.Raise();
	//	gameManager.SelectedUnit.onChangeTarget.Raise();

	//	return gameManager.SelectedUnit;
	//}

	//public override void Update(SP_GameStateManager gameManager)
	//{
	//	gameManager.SelectedUnit.currentPos = NodeGrid.Instance.getNodeFromTransformPosition(gameManager.SelectedUnit.transform);
	//	if (Input.GetKeyDown(KeyCode.Tab))
	//	{
	//		SelectNextPlayer(gameManager);
	//	}

	//	//gameManager.SelectedPlayer.checkFlank(gameManager?.SelectedEnemy?.currentPos);
	//}

	//public override void ExitState(SP_GameStateManager gameManager)
	//{
	//	if (gameManager.SelectedUnit != null)
	//	{
	//		gameManager.SelectedUnit.enabled = false;
	//		gameManager.SelectedUnit.fpsCam.enabled = false;
	//		//Debug.Log($"exit State {nameof(PlayerTurn)}");
	//		gameManager.clearPreviousSelectedUnitFromAllWeaponEvent(gameManager.SelectedUnit?.CurrentTarget);
	//	}
	//}

	//public void SelectNextPlayer(SP_GameStateManager gameManager)
	//{
	//	int nbPlayers = gameManager.players.Count;

	//	if (gameManager != null)
	//	{
	//		gameManager.SelectedUnit.enabled = false;
	//		gameManager.SelectedUnit.SwitchState(gameManager.SelectedUnit.idelState);
	//		gameManager.SelectedUnit.fpsCam.enabled = false;

	//		//List<PlayerStateManager> availablePlayers = gameManager.players.Where(unit => unit.State is Idel).ToList();
	//		int currentPlayerIndex = gameManager.players.FindIndex(instance => instance == gameManager.SelectedUnit);
	//		gameManager.SelectedUnit = gameManager.players[(currentPlayerIndex + 1) % nbPlayers];

	//		gameManager.SelectedUnit.enabled = true;
	//		gameManager.SelectedUnit.fpsCam.enabled = true;
	//		gameManager.SelectedUnit.onCameraEnabeled();
	//		gameManager.SelectedUnit.CurrentTarget = gameManager.enemies.FirstOrDefault(unit => unit.State is Idel);
	//		//Vector3 TargetDir = gameManager.SelectedUnit.CurrentTarget.aimPoint.position - gameManager.SelectedUnit.aimPoint.position;
	//		//await gameManager.SelectedUnit.rotateTowardDirection(gameManager.SelectedUnit.partToRotate, TargetDir, 3);
	//		//await gameManager.SelectedUnit.rotateTowardDirection(gameManager.SelectedUnit.CurrentTarget.partToRotate, -TargetDir, 3);
	//		//gameManager.SelectedUnit.newFlunking(gameManager.SelectedUnit.CurrentTarget);
	//		//gameManager.MakeGAmeMAnagerListingToNewSelectedUnit(gameManager.SelectedPlayer);

	//		//gameManager.PlayerChangeEvent.Raise();
	//		//gameManager.SelectedUnit.CoverBihaviour.UpdateNorthPositionTowardTarget(gameManager.SelectedUnit.CurrentTarget);
	//		//gameManager.SelectedUnit.CoverBihaviour.CalculateCoverValue();
	//		//gameManager.SelectedUnit.CheckForFlunks();
	//	}
	//}
	public override void EnterState(SP_GameStateManager Context)
	{
		throw new System.NotImplementedException();
	}

	public override void ExitState(SP_GameStateManager Context)
	{
		throw new System.NotImplementedException();
	}

	public override void Update(SP_GameStateManager Context)
	{
		throw new System.NotImplementedException();
	}
}

public class EnemyTurn : BaseState<SP_GameStateManager>
{
	//public override AnyClass EnterState(SP_GameStateManager gameManager)
	//{
	//	gameManager.SelectedUnit = gameManager.enemies.FirstOrDefault();
	//	gameManager.SelectedUnit.CurrentTarget = gameManager.players.FirstOrDefault();
	//	//gameManager.SelectedEnemy.currentPos = gameManager.SelectedEnemy.grid.getNodeFromTransformPosition(gameManager.SelectedEnemy.transform);

	//	gameManager.SelectedUnit.enabled = true;
	//	gameManager.SelectedUnit.fpsCam.enabled = true;
	//	gameManager.SelectedUnit.onCameraEnabeled();

	//	//gameManager.PlayerChangeEvent.Raise();
	//	gameManager.SelectedUnit.onChangeTarget.Raise();
	//	return gameManager.SelectedUnit;
	//}

	//public override void Update(GameStateManager gameManager)
	//{
	//	gameManager.SelectedUnit.currentPos = NodeGrid.Instance.getNodeFromTransformPosition(gameManager.SelectedUnit.transform);
	//	if (Input.GetKeyDown(KeyCode.Tab))
	//	{
	//		SelectNextEnemy(gameManager);
	//	}

	//	//gameManager.SelectedPlayer.checkFlank(gameManager?.SelectedEnemy?.currentPos);
	//}

	//public override void ExitState(GameStateManager gameManager)
	//{
	//	if (gameManager.SelectedUnit != null)
	//	{
	//		gameManager.SelectedUnit.enabled = false;
	//		gameManager.SelectedUnit.fpsCam.enabled = false;
	//		gameManager.clearPreviousSelectedUnitFromAllWeaponEvent(gameManager.SelectedUnit?.CurrentTarget);
	//	}
	//}

	//public void SelectNextEnemy(GameStateManager gameManager)
	//{
	//	int nbEnemies = gameManager.enemies.Count;

	//	if (gameManager != null)
	//	{
	//		int nbPlayers = gameManager.enemies.Count;

	//		gameManager.SelectedUnit.enabled = false;
	//		gameManager.SelectedUnit.SwitchState(gameManager.SelectedUnit.idelState);
	//		gameManager.SelectedUnit.fpsCam.enabled = false;
	//		int currentPlayerIndex = gameManager.enemies.FindIndex(instance => instance == gameManager.SelectedUnit);
	//		gameManager.SelectedUnit = gameManager.enemies[currentPlayerIndex % nbPlayers];

	//		gameManager.SelectedUnit.enabled = true;
	//		gameManager.SelectedUnit.fpsCam.enabled = true;
	//		gameManager.SelectedUnit.onCameraEnabeled();
	//		gameManager.SelectedUnit.CurrentTarget = gameManager.enemies.FirstOrDefault(unit => unit.State is Idel);
	//	}
	//}
	public override void EnterState(SP_GameStateManager Context)
	{
		throw new System.NotImplementedException();
	}

	public override void ExitState(SP_GameStateManager Context)
	{
		throw new System.NotImplementedException();
	}

	public override void Update(SP_GameStateManager Context)
	{
		throw new System.NotImplementedException();
	}
}
public abstract class BaseState<T>
{
	public string name;

	public abstract void EnterState(T Context);

	public abstract void Update(T Context);

	public abstract void ExitState(T Context);

	public override string ToString()
	{
		return $"{this.GetType().Name}";
	}
}