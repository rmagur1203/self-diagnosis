const requestp = require("request-promise");
const request = require("request");
const JSEncrypt = require('node-jsencrypt');
const crypto = new JSEncrypt();

crypto.setPublicKey("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA81dCnCKt0NVH7j5Oh2+SGgEU0aqi5u6sYXemouJWXOlZO3jqDsHYM1qfEjVvCOmeoMNFXYSXdNhflU7mjWP8jWUmkYIQ8o3FGqMzsMTNxr+bAp0cULWu9eYmycjJwWIxxB7vUwvpEUNicgW7v5nCwmF5HS33Hmn7yDzcfjfBs99K5xJEppHG0qc+q3YXxxPpwZNIRFn0Wtxt0Muh1U8avvWyw03uQ/wMBnzhwUC8T4G5NclLEWzOQExbQ4oDlZBv8BM/WxxuOyu0I8bDUDdutJOfREYRZBlazFHvRKNNQQD2qDfjRz484uFs7b5nykjaMB9k/EJAuHjJzGs9MMMWtQIDAQAB");

let atptOfcdcConctUrl = "hcs.eduro.go.kr";

const lctnScCodes = Object.freeze({
    서울특별시: 01,
    부산광역시: 02,
    대구광역시: 03,
    인천광역시: 04,
    광주광역시: 05,
    대전광역시: 06,
    울산광역시: 07,
    세종특별자치시: 08,
    경기도: 10,
    강원도: 11,
    충청북도: 12,
    충청남도: 13,
    전라북도: 14,
    전라남도: 15,
    경상북도: 16,
    경상남도: 17,
    제주특별자치도: 18
});
const schulCrseScCodes = Object.freeze({
    유치원: 1,
    초등학교: 2,
    중학교: 3,
    고등학교: 4,
    특수학교: 5
});
const normalServey = {
    rspns00: "Y",
    //학생의 몸에 열이 있나요?
    //Y. 37.5℃ 미만
    //N.37.5℃ 이상 및 발열감
    rspns01: "1",
    //Unknown
    //학생에게 코로나19가 의심되는 증상이 있나요 ?
    rspns02: "1",
    //아니요
    rspns03: null,
    //기침
    rspns04: null,
    //인후통
    rspns05: null,
    //호흡곤란
    rspns06: null,
    //Unknown
    rspns07: "0",
    //학생이 최근(14일 이내) 해외여행을 다녀온 사실이 있나요?
    rspns08: "0",
    //동거가족 중 최근(14일 이내) 해외여행을 다녀온 사실이 있나요?
    rspns09: "0",
    //동거가족 중 현재 자가격리 중 인 가족이 있나요?
    rspns10: null,
    //Unknown
    rspns11: null,
    //후각·미각 소실
    rspns12: null,
    //Unknown
    rspns13: null,
    //오한
    rspns14: null,
    //근육통
    rspns15: null,
    //두통
    deviceUuid: ""
};
const URL_LIST = {
    SEARCH_SCHOOL: "/v2/searchSchool",
    FIND_USER: "/v2/findUser",
    UPDATE_AGREEMENT: "/v2/updatePInfAgrmYn",
    HAS_PASSWORD: "/v2/hasPassword",
    REGISTER_PASSWORD: "/v2/registerPassword",
    LOGIN_WITH_SECOND_PASSWORD: "/v2/validatePassword",
    INIT_PASSWORD: "/v2/initPassword",
    SEND_SURVEY_RESULT: "/registerServey",
    SELECT_GROUP_LIST: "/v2/selectUserGroup",
    REFRESH_USER_INFO: "/v2/getUserInfo",
    REGISTER_GROUP_LIST: "/v2/insertUserInGroup",
    REMOVE_GROUP_LIST: "/v2/removeUserInGroup",
    TEACHER_CLASS_LIST: "/joinClassList",
    MANAGER_CLASS_LIST: "/joinDeptList",
    CLASS_JOIN_LIST: "/join",
    DEPT_JOIN_LIST: "/joinTchr",
    JOIN_DETAIL: "/joinDetail",
    SEND_PUSH: "/push",
    SELECT_NOTICE_LIST: "/v2/selectNoticeList",
    SELECT_NOTICE_CONTENTS: "/v2/selectNotice"
}

const v1 = {
    WAFData: () => new Promise((resolve, reject) =>
        request.get(`https://${atptOfcdcConctUrl}/`, {
            headers: {
                'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36',
                'Content-Type': 'application/json',
            },
        }, function(err, res, body) {
            if (err) reject(err);
            resolve(res.headers["set-cookie"]);
        })),
    selectGroupList: (token) => new Promise((resolve, reject) =>
        request.post({
            url: `https://${atptOfcdcConctUrl}/selectGroupList`,
            method: "POST",
            headers: {
                'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36',
                'Authorization': token,
                'Origin': 'https://hcs.eduro.go.kr',
                'Referer': 'https://hcs.eduro.go.kr/'
            },
            json: {}
        }, function(err, res, body) {
            if (err) reject(err);
            resolve(body);
        })),
    UserRefresh: (token, orgCode, userPNo) => new Promise((resolve, reject) =>
        request.post({
            url: `https://${atptOfcdcConctUrl}/userrefresh`,
            method: "POST",
            headers: {
                'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36',
                'Authorization': token,
                'Origin': 'https://hcs.eduro.go.kr',
                'Referer': 'https://hcs.eduro.go.kr/'
            },
            json: {
                orgCode: orgCode,
                userPNo: userPNo
            }
        }, function(err, res, body) {
            if (err) reject(err);
            resolve(body);
        })),
    SearchSchool: (lctnScCode, schulCrseScCode, orgName) => new Promise(async(resolve, reject) => {
        console.warn("v1 의 SearchSchool 은 Deprecated 되었습니다. 대신 v2 의 SearchSchool 을 사용해주세요.");
        resolve(await v2.searchSchool(lctnScCode, schulCrseScCode, orgName));
    }),
    LoginToken: (orgcode, name, birthday) => new Promise((resolve, reject) =>
        request.post({
            url: `https://${atptOfcdcConctUrl}/loginwithschool`,
            method: "POST",
            headers: {
                'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36',
                'Content-Type': 'application/json',
                'Origin': 'https://hcs.eduro.go.kr',
                'Referer': 'https://hcs.eduro.go.kr/'
            },
            json: {
                orgcode: orgcode,
                name: crypto.encrypt(name),
                birthday: crypto.encrypt(birthday)
            }
        }, function(err, res, body) {
            if (err) reject(err);
            resolve(body);
        })),
    Servey: (token, form = normalServey) => new Promise((resolve, reject) =>
        request.post({
            url: `https://${atptOfcdcConctUrl}/registerServey`,
            method: "POST",
            headers: {
                'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36',
                'Authorization': token,
                'Origin': 'https://hcs.eduro.go.kr',
                'Referer': 'https://hcs.eduro.go.kr/'
            },
            json: form
        }, function(err, res, body) {
            if (err) reject(err);
            resolve(body);
        })),
    AutoCheck: async(lctnScCode, schulCrseScCode, schoolName, Name, Birth) => {
        var School = await SearchSchool(lctnScCode, schulCrseScCode, schoolName);
        var Token = await LoginToken(School.schulList[0].orgCode, Name, Birth);
        var servey = await Servey(Token.token);
        return JSON.stringify(servey);
    }
}

const v2 = {
    searchSchool: (lctnScCode, schulCrseScCode, orgName, loginType = "school") => new Promise((resolve, reject) => {
        var url = encodeURI(`https://${atptOfcdcConctUrl}/v2/searchSchool?lctnScCode=${lctnScCode}&schulCrseScCode=${schulCrseScCode}&orgName=${orgName}&loginType=${loginType}`);
        request.get(url, function(err, res, body) {
            if (err) reject(err);
            let School = JSON.parse(body);
            atptOfcdcConctUrl = School.schulList[0].atptOfcdcConctUrl;
            resolve(School);
        });
    }),
    findUser: (orgcode, name, birthday, loginType = "school") => new Promise((resolve, reject) =>
        request.post({
            url: `https://${atptOfcdcConctUrl}/v2/findUser`,
            method: "POST",
            headers: {
                'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36',
                'Content-Type': 'application/json',
                'Origin': 'https://hcs.eduro.go.kr',
                'Referer': 'https://hcs.eduro.go.kr/'
            },
            json: {
                orgCode: orgcode,
                name: crypto.encrypt(name),
                birthday: crypto.encrypt(birthday),
                loginType: loginType,
                stdntPNo: null
            }
        }, function(err, res, body) {
            if (err) reject(err);
            resolve(body);
        })),
    hasPassword: (token) => new Promise((resolve, reject) => request.post({
        url: `https://${atptOfcdcConctUrl}/v2/hasPassword`,
        method: "POST",
        headers: {
            'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36',
            'Content-Type': 'application/json',
            'Authorization': token,
            'Origin': 'https://hcs.eduro.go.kr',
            'Referer': 'https://hcs.eduro.go.kr/'
        },
        json: {}
    }, function(err, res, body) {
        if (err) reject(err);
        resolve(body);
    })),
    validatePassword: (token, password) => new Promise((resolve, reject) => request.post({
        url: `https://${atptOfcdcConctUrl}/v2/validatePassword`,
        method: "POST",
        headers: {
            'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36',
            'Content-Type': 'application/json',
            'Authorization': token,
            'Origin': 'https://hcs.eduro.go.kr',
            'Referer': 'https://hcs.eduro.go.kr/'
        },
        json: {
            deviceUuid: "",
            password: crypto.encrypt(password)
        }
    }, function(err, res, body) {
        if (err) reject(err);
        resolve(body);
    })),
    selectUserGroup: (token) => new Promise((resolve, reject) => request.post({
        url: `https://${atptOfcdcConctUrl}/v2/selectUserGroup`,
        method: "POST",
        headers: {
            'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36',
            'Content-Type': 'application/json',
            'Authorization': token,
            'Origin': 'https://hcs.eduro.go.kr',
            'Referer': 'https://hcs.eduro.go.kr/'
        },
        json: {}
    }, function(err, res, body) {
        if (err) reject(err);
        resolve(body);
    })),
    getUserInfo: (token, orgCode, userPNo) => new Promise((resolve, reject) => request.post({
        url: `https://${atptOfcdcConctUrl}/v2/getUserInfo`,
        method: "POST",
        headers: {
            'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36',
            'Content-Type': 'application/json',
            'Authorization': token,
            'Origin': 'https://hcs.eduro.go.kr',
            'Referer': 'https://hcs.eduro.go.kr/'
        },
        json: {
            orgCode: orgCode,
            userPNo: userPNo
        }
    }, function(err, res, body) {
        if (err) reject(err);
        resolve(body);
    })),
    registerServey: async(token, form = normalServey) => {
        console.warn("Deprecated warning in v2.registerServey function\n-   v1 의 registerServey 를 사용해주시기 바랍니다.");
        return await v1.Servey(token, form);
    }
}

module.exports = {
    atptOfcdcConctUrl: atptOfcdcConctUrl,
    lctnScCodes: lctnScCodes,
    schulCrseScCodes: schulCrseScCodes,
    v1: v1,
    v2: v2
}