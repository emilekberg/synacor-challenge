// start = 05452 :   jf $8 05605
var $1,$2,$3,$4,$5,$6,$7,$8;

const push = (param) => {};
const pop = (param) => {};
const set = (a,b) => {};
const jump = (param) => {};
const add = (a,b,c) => {};
const rmem = (a,b) => {};
const gt = (a,b,c) => {};
const jt = (a,b,c) => {};
const call_01458 = () =>
{
  push($1);
  push($4);
  push($5);
  push($6);
  push($7);
   set($7, $1)
   set($6, $2)
  rmem($5, $1)
   set($2, 00000)
   add($4, 00001, $2);
    gt($1, $4, $5);
    if($1 != 0)
    {
      jump(01507);
    }
   add $4 $4 $7
  rmem $1 $4
  call $6
   add $2 $2 00001
  if($2 != 0)
  {
    jump(01480);
  }
   pop($7);
   pop($6);
   pop($5);
   pop($4);
   pop($1);
   ret
  push($2);
   set($2, 01528);
   call_01458();
};
if($8 == 0)
{
  teleport();
  return;
}

push($1);
push($2);
push($3);

$1 = 28844;
$2 = 1531;
$3 = (26275 + 5837) % 32768;

// 05515 : call 01458
call_01458();

pop($3);
pop($2);
pop($1);