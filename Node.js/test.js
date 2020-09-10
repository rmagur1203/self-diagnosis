const Diagnosis = require('./index');
const readline = require('readline');
const { writeFileSync, existsSync, readFileSync } = require('fs');

const input = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

const exit = async() => {
    process.stdin.setRawMode(true);
    return new Promise(resolve => process.stdin.once('data', data => {
        process.exit(1);
    }));
}

async function Check(lctnScCode, schulCrseScCode, schoolName, Name, Birth) {
    try {
        var School = await Diagnosis.SearchSchool(lctnScCode, schulCrseScCode, schoolName);
        var Token = await Diagnosis.LoginToken(School.schulList[0].orgCode, Name, Birth);
        var List = await Diagnosis.selectGroupList(Token.token);
        var User = await Diagnosis.UserRefresh(Token.token, School.schulList[0].orgCode, List.groupList[0].userPNo);
        var servey = "오늘은 이미 자가진단을 했습니다.";
        if (User.UserInfo.registerDtm != undefined) {
            var last = new Date(User.UserInfo.registerDtm);
            if (!sameDay(last, new Date())) {
                servey = await Diagnosis.Servey(Token.token);
            }
        } else {
            servey = await Diagnosis.Servey(Token.token);
        }
        console.log(servey);
        console.log("계속하려면 아무키나 누르십시오...");
        await exit();
    } catch (ex) {
        console.log("오류가 발생했습니다.");
        console.log(ex);
    }
}

sameDay = (d1, d2) => d1.getFullYear() === d2.getFullYear() && d1.getMonth() === d2.getMonth() && d1.getDate() === d2.getDate();

if (existsSync("info.json")) {
    (async() => {
        var obj = JSON.parse(await readFileSync("info.json"));
        Check(obj.lctnScCode, obj.schulCrseScCode, obj.schoolName, obj.Name, obj.Birth);
    })();
} else {
    console.clear();
    var lctns = Object.getOwnPropertyNames(Diagnosis.lctnScCodes);
    var schul = Object.getOwnPropertyNames(Diagnosis.schulCrseScCodes);

    for (var i = 0; i < lctns.length; i++) {
        console.log(i + ": " + lctns[i]);
    }

    input.question("학교 지역을 골라주세요: ", function(answer1) {
        console.clear();
        console.log("지역:", lctns[answer1]);
        for (var i = 0; i < schul.length; i++) {
            console.log(i + ": " + schul[i]);
        }
        input.question("학교의 수준을 골라주세요: ", function(answer2) {
            console.clear();
            console.log("지역:", lctns[answer1]);
            console.log("구분:", schul[answer2]);
            input.question("학교 이름을 입력해주세요: ", function(answer3) {
                console.clear();
                console.log("지역:", lctns[answer1]);
                console.log("구분:", schul[answer2]);
                console.log("학교 이름:", answer3);
                input.question("이름을 입력해주세요: ", function(answer4) {
                    console.clear();
                    console.log("지역:", lctns[answer1]);
                    console.log("구분:", schul[answer2]);
                    console.log("학교 이름:", answer3);
                    console.log("이름:", answer4);
                    input.question("생년월일을 입력해주세요 (년년월월일일): ", function(answer5) {
                        console.clear();
                        console.log("지역:", lctns[answer1]);
                        console.log("구분:", schul[answer2]);
                        console.log("학교 이름:", answer3);
                        console.log("이름:", answer4);
                        console.log("생년월일:", answer5);
                        var obj = {
                            lctnScCode: Diagnosis.lctnScCodes[lctns[answer1]],
                            schulCrseScCode: Diagnosis.schulCrseScCodes[schul[answer2]],
                            schoolName: answer3,
                            Name: answer4,
                            Birth: answer5
                        }
                        writeFileSync("info.json", JSON.stringify(obj));
                        Check(obj.lctnScCode, obj.schulCrseScCode, obj.schoolName, obj.Name, obj.Birth);
                    });
                });
            });
        });
    });
}