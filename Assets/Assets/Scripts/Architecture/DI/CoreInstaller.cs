﻿using Core.GameState;
using Core.GameState.States;
using Core.Services.DataRepository;
using Core.Services.Validation;
using Core.Systems.SessionManage;
using Core.Systems.WordContainer;
using Infrastructure;
using Infrastructure.RemoteConfig;
using Zenject;

namespace Architecture.DI
{
    public sealed class CoreInstaller : MonoInstaller<CoreInstaller>
    {
        public override void InstallBindings()
        {
            BindDataRepository();

            BindInitSystems();

            BindTrackers();
            
            BindGameStates();

            BindServices();
            
            BindEntryPoint();
        }

        private void BindDataRepository()
        {
            Container.BindInterfacesTo<GameDataRepository>().AsSingle();
        }

        private void BindInitSystems()
        {
            Container.BindInterfacesTo<RemoteLevelLoader>().AsSingle();
            Container.BindInterfacesTo<LevelProcessor>().AsSingle();
        }

        private void BindTrackers()
        {
            Container.BindInterfacesAndSelfTo<WordRepositoryTracker>().AsSingle();
        }
        
        private void BindGameStates()
        {
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<MainMenuState>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelGenerationState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayState>().AsSingle();
            Container.BindInterfacesAndSelfTo<VictoryState>().AsSingle();
        }

        private void BindServices()
        {
            Container.BindInterfacesTo<ValidationService>().AsSingle();
            Container.BindInterfacesTo<GameSessionCleaner>().AsSingle();
        }

        private void BindEntryPoint()
        {
            Container.BindInterfacesAndSelfTo<AppStartup>().AsSingle().NonLazy();
        }
    }
}