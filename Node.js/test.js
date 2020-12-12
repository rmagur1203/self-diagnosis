const Diagnosis = require('./index');
const readline = require('readline');
const { writeFileSync, existsSync, readFileSync } = require('fs');

const input = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});
const question = (query) => new Promise((resolve, reject) => {
    input.question(query, function(answer) {
        resolve(answer);
    });
});

const exit = async() => {
    process.stdin.setRawMode(true);
    return new Promise(resolve => process.stdin.once('data', data => {
        process.exit(1);
    }));
}

async function Check(orgCode, cdcUrl, Name, Birth) {
    try {
        Diagnosis.setCdcUrl(cdcUrl);
        var Token = await Diagnosis.v2.findUser(orgCode, Name, Birth);
        var List = await Diagnosis.v2.selectUserGroup(Token.token);
        var User = await Diagnosis.v2.getUserInfo(List[0].token, orgCode, List[0].userPNo);
        var servey = "오늘은 이미 자가진단을 했습니다.";
        if (User.registerDtm != undefined) {
            var last = new Date(User.registerDtm);
            if (!sameDay(last, new Date())) {
                servey = await Diagnosis.v2.registerServey(User.token);
            }
        } else {
            servey = await Diagnosis.v2.registerServey(User.token);
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

(async() => {
    if (existsSync("info.json")) {
        (async() => {
            var obj = JSON.parse(await readFileSync("info.json"));
            Check(obj.orgCode, obj.cdcUrl, obj.Name, obj.Birth);
        })();
    } else {
        console.clear();
        var lctns = Object.getOwnPropertyNames(Diagnosis.lctnScCodes);
        var schul = Object.getOwnPropertyNames(Diagnosis.schulCrseScCodes);
        var lctnans, schulans, schdata, name, birth;

        while (true) {
            for (var i = 0; i < lctns.length; i++) {
                console.log(i + ": " + lctns[i]);
            }
            var answer = await question("학교 지역을 골라주세요: ");
            console.clear();
            if (isNaN(parseInt(answer))) continue;
            lctnans = lctns[answer];
            break;
        }
        while (true) {
            console.log("지역:", lctnans);
            for (var i = 0; i < schul.length; i++) {
                console.log(i + ": " + schul[i]);
            }
            var answer = await question("학교의 수준을 골라주세요: ");
            console.clear();
            if (isNaN(parseInt(answer))) continue;
            schulans = schul[answer];
            break;
        }
        while (true) {
            console.log("지역:", lctnans);
            console.log("구분:", schulans);
            var answer = await question("학교 이름을 입력해주세요: ");
            console.clear();
            var schoolData = await Diagnosis.v2.searchSchool(Diagnosis.lctnScCodes[lctnans], Diagnosis.schulCrseScCodes[schulans], answer).catch((ex) => console.log(ex));
            for (var i = 0; i < schoolData.schulList.length; i++) {
                var school = schoolData.schulList[i];
                console.log(`${i}: ${school.kraOrgNm} - ${school.addres}`);
            }
            var number = await question("학교를 선택하세요: ");
            if (isNaN(parseInt(number))) continue;
            schdata = schoolData.schulList[number];
            break;
        }
        console.clear();
        console.log("지역:", lctnans);
        console.log("구분:", schulans);
        console.log("학교 이름:", schdata.kraOrgNm);

        name = await question("이름을 입력해주세요: ");
        console.clear();
        console.log("지역:", lctnans);
        console.log("구분:", schulans);
        console.log("학교 이름:", schdata.kraOrgNm);
        console.log("이름:", name);

        birth = await question("생년월일을 입력해주세요 (년년월월일일): ");
        console.clear();
        console.log("지역:", lctnans);
        console.log("구분:", schulans);
        console.log("학교 이름:", schdata.kraOrgNm);
        console.log("이름:", name);
        console.log("생년월일:", birth);

        var obj = {
            orgCode: schdata.orgCode,
            cdcUrl: schdata.atptOfcdcConctUrl,
            Name: name,
            Birth: birth
        }
        writeFileSync("info.json", JSON.stringify(obj));
        Check(obj.orgCode, obj.cdcUrl, obj.Name, obj.Birth);
    }
})();