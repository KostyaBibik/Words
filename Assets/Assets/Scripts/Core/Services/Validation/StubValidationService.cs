using Assets.Scripts.Core.Services.Abstract;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Services.Validation
{
    public class StubValidationService : IValidationService
    {
        public async UniTask Validate()
        {
            Debug.Log("[ValidationService] Проверка игрового поля...");
            
            await UniTask.Delay(500); // Симулируем задержку
            
            Debug.Log("[ValidationService] Проверка завершена (заглушка)");
        }
    }
}