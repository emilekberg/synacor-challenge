# synacor-challenge

## Architecture of my implementation

The architecture from my interpretation of the VM uses a specific class for every operation. the class contains the code to execute it, how many parameters it has, and what the int code for this operation is. Example implementation:

```c#
public class Add : IOperation
{
  public ushort GetOpCode() => 9;
  public ushort GetNumParameters() => 3;
  public bool Execute(IVirtualMemory memory)
  {
    ushort a = memory.ReadNext();
    ushort b = memory.TryReadRegistry(memory.ReadNext());
    ushort c = memory.TryReadRegistry(memory.ReadNext());

    ushort result = (ushort)((b + c) % 32768);
    memory.Write(a, result);
    return true;
  }
}
```

Using reflection, i later find all of these implementations and register them to the DI System (Microsoft.Extensions.DependencyInjection), `see synacor_challange.Extensions.ServiceCollectionExtension`.
Using the DI system has two benefits. Easily we can inject things as loggers into the commands, but also it gets very easy to fetch all the values as an `IEnumerable<IOperation>`
In the VM these are later mapped to a dictionary instead of an IEnumerable, with the OpCode as the key, and the implementation as the value.

this makes it easy to extend the application with more operations, but also to call them.
```c#
var opCode= Memory.ReadNext();
if (!Operations.TryGetValue(opCode, out var operation))
{
  Logger.Error($"{opCode} not found amongst registered operations");
  continueRunning = false;
}
Logger.Debug($"{Memory.GetAddressPointer()}, opcode: {opCode} - {operation.GetType().Name}");
continueRunning = operation.Execute(Memory);
```

## Input

Upon request, the VM can ask for input from the user. These can either be fed by the keyboard, or by using a textfile, which will be converted into a `Queue<char>` when loaded. This makes it easy to switfly reach a certain point in the program.

## Compilation and Decompilation

Some form of compilation and decompilation has been written to make it easier to read, and manipulate programs. 
Example program might look like this:
```
21,21,19,87,19,101,19,108,19,99,19,111,19,109,19,101,19,32,19,116
```

where as my decompiled version will be more readable, 

```
00000 : noop
00001 : noop
00002 : out Welcome to the Synacor Challenge!\nPlease record your progress by putting codes like\nthis one into the challenge website: uxmvWPADwVwt\n\nExecuting self-test...\n\n
00320 :  jmp 00347
00322 : out jmp fails\n
```

some extra manipulation is made, for instance, writing text to console is made with one row instead of multiple ones. The VM can also compile these back into integer codes, making it easy to read, extract, update and use newer programs.

## Codes

### One
The first code is found by opening the arch-spec file.

```
- Here's a code for the challenge website: GhfjSUQLRNQa
```

### Two
The code is found by figuring out that you should implement some basic commands as given in the arch-spec, but also to load the binary file, and running it in the virtual machine.
```c#
public static ushort[] BinaryToProgram(string path)
{
  byte[] data = File.ReadAllBytes(path);
  ushort[] program = new ushort[(int)Math.Ceiling(data.Length / 2m)];
  Buffer.BlockCopy(data, 0, program, 0, data.Length);

  return program;
}
```

```
Welcome to the Synacor Challenge!
Please record your progress by putting codes like
this one into the challenge website: uxmvWPADwVwt
```

### Three
The code is found by implementing all operations, making the VM Self-Test pass.
```
Executing self-test...

self-test complete, all tests pass
The self-test completion code is: qnoLQtkJKZWr
```


### Four
The code is found by picking up a tablet and using it.
```
use tablet

You find yourself writing "LedsPGZkreJM" on the tablet.  Perhaps it's some kind of code?
```

### Five

The code is found in the maze, by walking 
- west
- south
- north

```
Chiseled on the wall of one of the passageways, you see:

    gaslrFyYaLxP

You take note of this and keep walking.
```

Here you will also find the oil for a lantern.
### Six


The sixth puzzle is found by solving the coin puzzle.
You will reach a point where 5 coins are found.

```
red coin, corroded coin, shiny coin, concave coin, blue coin.
```

These also has a certain number mapped to them, found by writing for instance `look red coin`.
The mapping can be intepreted as this: 

```c#
var coinNames = new Dictionary<int, string>()
{
  [2] = "red coin",
  [3] = "corroded coin",
  [5] = "shiny coin",
  [7] = "concave coin",
  [9] = "blue coin"
};
```

These are used to solve the formula `_ + _ * _^2 + _^3 - _ = 399`.
I created a program to brute force this, CoinBruteForcer. 
The application will simply brute force all different permutations of the values and find the correct order which calculates to 399 and output this to the console.

```
2 + 3 * 5^2 + 7^2 - 9 = 411
...
9 + 2 * 5^2 + 3^2 - 7 = 79
9 + 2 * 5^2 + 7^2 - 3 = 399
9,2,5,7,3 is the one
blue coin,red coin,shiny coin,concave coin,corroded coin is the one
```

After doing this you can find the teleporter, teleporting to the Synacor headquarters and find the code.

```
You activate the teleporter!  As you spiral through time and space, you think you see a pattern in the stars...

    GVuhIJkvgPKX
```

### Seven

In the synacor headquarters, you will find a strange journal, describing your means to unlocking the teleporters. 
In short, this is done by changing the value for the 8th register. My first thought was to change this value to random value and try it.
In the decompiled file we can see the row

```
05451 :   jf $7 05605
```

which jumps if $7 is zero. If we change this to some other value, say 12345, we get stuck in a hefty verification algorithm, and abort ed after some time. My second thought was to disable the verification algorithm by replacing it with noops. 

```
05489 : call 06027
```
is replaced with
```
05489 : noop
05490 : noop
```

While this works. The code given on the beach will be incorrect. We must find the correct value to put in the 8th register.
To do this, i reversed engineered the algorithm which can be found in the decompiled code on row `06027`
the full code it self is 

```
06027 :   jt $0 06035
06030 :  add $0 $1 00001
06034 :  ret
06035 :   jt $1 06048
06038 :  add $0 $0 32767
06042 :  set $1 $7
06045 : call 06027
06047 :  ret
06048 : push $0
06050 :  add $1 $1 32767
06054 : call 06027
06056 :  set $1 $0
06059 :  pop $0
06061 :  add $0 $0 32767
06065 : call 06027
06067 :  ret
```

written as C# this can be seen as 

```c#
public static void Address06027(ushort[] reg, Stack<ushort> stack)
{
  if (reg[0] == 0)
  {
    reg[0] = (ushort)((reg[1] + 1) % 32768);
    return;
  }
  if (reg[1] == 0)
  {
    reg[0] = (ushort)((reg[0] + 32767) % 32768);
    reg[1] = reg[7];
    Address06027(reg, stack);
    return;
  }
  stack.Push(reg[0]);
  reg[1] = (ushort)((reg[1] + 32767) % 32768);
  Address06027(reg, stack);
  reg[1] = reg[0];
  reg[0] = (ushort)(stack.Pop());
  reg[0] = (ushort)((reg[0] + 32767) % 32768);
  Address06027(reg, stack);
}
```

this code is very slow and will nest very deep, to deep actually to be run successfully. To solve this, i rewrote it to use memoization and a dictionary. 

```c#
public static Dictionary<Registry, Registry> Memoization = new();
public static Registry Address06027(Registry registry)
{
  if(Memoization.TryGetValue(registry, out var result))
  {
    return result;
  }
  if(registry.R0 == 0)
  {
    result = registry with
    {
      R0 = Add(registry.R1, 1)
    };
    Memoization.Add(registry, result);
    return result;
  }
  if(registry.R1 == 0)
  {
    result = Address06027(registry with
    {
      R0 = SubtractOne(registry.R0),
      R1 = registry.R7
    });
    Memoization.Add(registry, result);
    return result;
  }
  ushort registryOne = Address06027(registry with
  {
    R1 = SubtractOne(registry.R1)
  }).R0;
  result = Address06027(registry with
  {
    R0 = SubtractOne(registry.R0),
    R1 = registryOne
  });
  Memoization.Add(registry, result);
  return result;
}
```

Later on calling this until we find a proper value for the 8th register.
```c#
public static void DoWork()
{
  bool succeded;
  ushort i = 1;
  Registry reg = new Registry(4, 1, i);
  do
  {
    Memoization.Clear();
    var res = Address06027(reg with { R7 = i});
    if(res.R0 != 6)
    {
      succeded = false;
    }
    else
    {
      succeded = true;
      Console.WriteLine($"result is {res}");
      Console.WriteLine($"Set registry 7 to {i} to teleport correctly");
    }
    i++;
  } while (!succeded);
}
```

when the result of registry 0 is 6, we know the proper value for the 8th register is found. this can also be seen in the decompiled source code, when we call the verification logic: 

```
05489 : call 06027
05491 :   eq $1 $0 00006
05495 :   jf $1 05579
```


the values needed found in my applications are

```
result is Registry { R0 = 6, R1 = 5, R7 = 25734 }
Set registry 7 to 25734 to teleport correctly
```

changing the source code, or injecting these values when using the teleport will let you pass the puzzle, and land on a beach instead.

```
A strange, electronic voice is projected into your mind:

  "Unusual setting detected!  Starting confirmation process!  Estimated time to completion: 1 billion years."

You wake up on a sandy beach with a slight headache.  The last thing you remember is activating that teleporter... but now you can't find it anywhere in your pack.  Someone seems to have drawn a message in the sand here:

    xicSDxfoukhr

It begins to rain.  The message washes away.  You take a deep breath and feel firmly grounded in reality as the effects of the teleportation wear off.
```

### Eight

The eith puzzle is still ongoing...