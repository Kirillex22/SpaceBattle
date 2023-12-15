using Hwdtech;
using Hwdtech.Ioc;
using SpaceBattle.Lib;
using System.Collections;
using Moq;

namespace SpaceBattle.Tests;

public class CollisionCheckCommandTest
{
    public CollisionCheckCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();   

        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
             IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        var concreteTree = new Hashtable(){
                {0, new Hashtable(){
                    {1, new Hashtable(){
                        {0, new Hashtable(){
                            {-1, new Hashtable()}
                    }
                }
                }
            }
            }
        }
        };

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Struct.CollisionTree",
            (object[] args) => concreteTree
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.UObject.GetProperty",
            (object[] args) => {
                var obj = (IUObject)args[0];
                var key = (string)args[1];
                return obj.GetProperty(key);
            }
        ).Execute();
    }

    [Fact]
    public void SuccefulExecutingWithCollision()
    {       
        var collisionCommand = new Mock<SpaceBattle.Lib.ICommand>();
        collisionCommand.Setup(c => c.Execute()).Verifiable("collisionCommand wasn't called");

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Event.Collision",
            (object[] args) => collisionCommand.Object
        ).Execute();

        var obj1 = new Mock<IUObject>();
        var obj2 = new Mock<IUObject>();
        
        obj1.Setup(o => o.GetProperty("Position")).Returns(new int[] {0, 0});
        obj2.Setup(o => o.GetProperty("Position")).Returns(new int[] {0, 1});
        obj1.Setup(o => o.GetProperty("Velocity")).Returns(new int[] {0, 0});
        obj2.Setup(o => o.GetProperty("Velocity")).Returns(new int[] {0, -1});

        var ccm = new CheckCollisionCommand(obj1.Object, obj2.Object);

        ccm.Execute();

        collisionCommand.VerifyAll();       
    }

    [Fact]
    public void SuccefulExecutingWithoutCollision()
    {       
        var obj1 = new Mock<IUObject>();
        var obj2 = new Mock<IUObject>();
        
        obj1.Setup(o => o.GetProperty("Position")).Returns(new int[] {0, 0});
        obj2.Setup(o => o.GetProperty("Position")).Returns(new int[] {0, 0});
        obj1.Setup(o => o.GetProperty("Velocity")).Returns(new int[] {0, 0});
        obj2.Setup(o => o.GetProperty("Velocity")).Returns(new int[] {0, 0});

        var ccm = new CheckCollisionCommand(obj1.Object, obj2.Object);

        Assert.Throws<ArgumentException>(() => ccm.Execute());       
    }

    [Fact]
    public void UnableToReadUobject()
    {       
        var obj1 = new Mock<IUObject>();
        var obj2 = new Mock<IUObject>();
        
        obj1.Setup(o => o.GetProperty(It.IsAny<string>())).Returns((string k) => throw new Exception("empty uobject"));
        
        var ccm = new CheckCollisionCommand(obj1.Object, obj2.Object);

        var exc = Assert.Throws<Exception>(() => ccm.Execute());
        Assert.Equal("empty uobject", exc.Message);            
    }
}

