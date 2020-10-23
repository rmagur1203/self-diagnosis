const Diagnosis = require('./index');
const JSEncrypt = require('node-jsencrypt');
const crypto = new JSEncrypt();

crypto.setPublicKey("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA81dCnCKt0NVH7j5Oh2+SGgEU0aqi5u6sYXemouJWXOlZO3jqDsHYM1qfEjVvCOmeoMNFXYSXdNhflU7mjWP8jWUmkYIQ8o3FGqMzsMTNxr+bAp0cULWu9eYmycjJwWIxxB7vUwvpEUNicgW7v5nCwmF5HS33Hmn7yDzcfjfBs99K5xJEppHG0qc+q3YXxxPpwZNIRFn0Wtxt0Muh1U8avvWyw03uQ/wMBnzhwUC8T4G5NclLEWzOQExbQ4oDlZBv8BM/WxxuOyu0I8bDUDdutJOfREYRZBlazFHvRKNNQQD2qDfjRz484uFs7b5nykjaMB9k/EJAuHjJzGs9MMMWtQIDAQAB");
(async() => {
    var lctn = Diagnosis.lctnScCodes.경기도;
    var schul = Diagnosis.schulCrseScCodes.중학교;
    var schulData = await Diagnosis.v2.searchSchool(lctn, schul, "");
    var orgCode = schulData.schulList[0].orgCode;
    var userData = await Diagnosis.v2.findUser(orgCode, "", "");
    var hasPassword = await Diagnosis.v2.hasPassword(userData.token);
    var userGroup = await Diagnosis.v2.selectUserGroup(userData.token);
    var userPNo = userGroup[0].userPNo;
    var userInfo = await Diagnosis.v2.getUserInfo(userData.token, orgCode, userPNo);
    var registerServey = await Diagnosis.v2.registerServey(userData.token)
    console.log(registerServey);
})();