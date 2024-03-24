# MediatorPatternExample-FruitTreeGrowing-Prototype-with-Generic-Type-Finite-State-Machine
Mediator Pattern kullanarak geliştirilmiş bir meyve bahçesi prototipini içeren repo'dur.<br>
<b>Bekleyen geliştirmeler : </b><br>
Object Pool for Fruit Prefabs. <br>
Fruit Inventory<br><br>

Bu prototipte, Mediator Pattern kullanarak, meyveler belli bir state'e girdiğinde, tüm meyve ağaçlarına haber gitmesi ve ilgili meyve ağaçlarının spawn time'larının güncellenmesi yapısı implemente edilmiştir. Mediator Script'ini Scriptable Object şeklinde kullanarak daha modüler bir yapı hedeflenmiştir.<br>
Ayrıca meyvelerin bir lifecycle'ı implemente edilmiştir. Player ağaçtan düşen meyveleri toplayabilmektedir.<br><br>

---<b>MEDIATOR RELATED CLASSES</b>---<br>
- TreeMediator.cs => Bu Mediator'ı event'ler ile kullanıyoruz. Değişiklik dinlemek isteyen sınıflar Action'lara subscribe olur, event tetiklemek isteyen sınıflar ise OnSpawned, OnStartedRipening, OnDecayed gibi metodları kullanarak ilgili event'i tetikleyebilir. <br><br>

---<b>STATE MACHINE BASE CLASSES</b>---<br>
<b>Bu state machine'de Generic Enum Type State'ler üzerinden yeni statemachine'ler türetmek hedeflenmiştir.</b><br>
- <b>StateManager.cs</b> => Her yeni StateMachine StateManager sınıfını ilgili Enum type'ı ile miras alarak bu statemachine temel metodlarına erişim sağlayabilir.<br>
- <b>BaseState.cs</b> => BaseState'ler bu State'den türetilebilir. Modüler bir yapı için, BaseState class'ından kendi base state sınıfımızı yaratıp, ardından somut(concrete) sınıfları türetmek en doğrusu olacaktır. Örnek : BaseState(A) => FruitState(A) => FruitGrowingState(C)<br>
- <b>Context Classes</b> => Farkedileceği üzere, türettiğimiz her StateMachine'in(FruitStateMachine, TreeStateMachine) erişim sağladığı ve popüle ettiği bir Context sınıfı(TreeContext, FruitContext) mevcut. Bu context sınıflarını kullandık çünkü state machine sınıflarımızı temiz tutmak istiyoruz. Sadece State management yapması daha doğru olacaktır. Diğer logic işlemler veya gerekli data'ları Context class'lara pass ettik. <br>





