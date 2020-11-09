# self-diagnosis
(교육부 자가진단)자가진단 관련 라이브러리
[![Build status](https://ci.appveyor.com/api/projects/status/d0el54x5849l9jpf?svg=true)](https://ci.appveyor.com/project/rmagur1203/self-diagnosis)
![Node.js CI](https://github.com/rmagur1203/self-diagnosis/workflows/Node.js%20CI/badge.svg)

# 함수 반환값 (v1) (Deprecated)
## SearchSchool
```js
{
  schulList: [
    {
      orgCode: '학교 코드',
      kraOrgNm: '학교 한글 이름',
      engOrgNm: '학교 영어 이름',
      insttClsfCode: '5',
      lctnScCode: '10',
      lctnScNm: '경기도',
      sigCode: '41111',
      juOrgCode: 'J100000250',
      schulKndScCode: '03',
      orgAbrvNm01: '수원북중학교',
      orgAbrvNm02: '수원북중학교',
      orgUon: 'Y',
      updid: 'SYSTEM',
      mdfcDtm: '2020-08-19 20:17:42.0',
      atptOfcdcConctUrl: '관할 교육청 주소',
      addres: '학교 주소'
    }
  ],
  sizeover: false //검색 결과가 30개 초과일시 30개만 표시하고 sizeover 가 true 가 됨
}
```
## LoginToken
```js
{
  registerDtm: '2020-09-11 08:10:41.796889', //자가진단을 한 시간
  admnYn: 'N',
  orgname: '수원북중학교',
  registerYmd: '20200911', //자가진단 한 날짜
  mngrClassYn: 'N', //아마도 선생님인지 확인하는 값
  name: '이름',
  man: 'N', //아마도 남자 여자 구분 N 은 남자
  stdntYn: 'Y', //아마도 학생인지 확인하는 값
  infAgrmYn: 'Y',
  token: '토큰',
  mngrDeptYn: 'N',
  isHealthy: true //건강 상태
}
```
## selectGroupList
```js
{
  groupList: [
    {
      orgCode: '학교 코드',
      orgName: '학교 이름',
      userPNo: '유저 고유번호',
      userNameEncpt: '이름',
      stdntYn: 'Y', //아마도 학생인지 확인하는 값
      mngrYn: 'N', //아마도 선생님인지 확인하는 값
      schulCrseScCode: '3',
      lctnScCode: '10',
      token: '토큰',
      atptOfcdcConctUrl: '관할 교육청 주소',
      wrongPassCnt: 0,
      otherYn: 'N' //groupList 에 다른 사람이 포함되어 있는지
    }
  ]
}
```
## registerServey
```js
{
  registerDtm: '자가진단 한 시각',
  inveYmd: '자가진단 한 시각'
}
```

# 함수 반환값 (v2)
## searchSchool
```js
{
  schulList: [
    {
      orgCode: '학교 코드',
      kraOrgNm: '학교 한글 이름',
      engOrgNm: '학교 영어 이름',
      insttClsfCode: '5',
      lctnScCode: '10',
      lctnScNm: '경기도',
      sigCode: '41111',
      juOrgCode: 'J100000250',
      schulKndScCode: '03',
      orgAbrvNm01: '수원북중학교',
      orgAbrvNm02: '수원북중학교',
      orgUon: 'Y',
      updid: 'SYSTEM',
      mdfcDtm: '2020-08-19 20:17:42.0',
      atptOfcdcConctUrl: '관할 교육청 주소',
      addres: '학교 주소'
    }
  ],
  sizeover: false //검색 결과가 30개 초과일시 30개만 표시하고 sizeover 가 true 가 됨
}
```
## findUser
```js
{
  orgName: '학교 이름',
  admnYn: 'N',
  atptOfcdcConctUrl: '관할 교육청 주소',
  mngrClassYn: 'N',
  pInfAgrmYn: 'Y',
  userName: '이름',
  stdntYn: 'Y',
  token: 'Bearer 토큰',
  mngrDeptYn: 'N'
}
```
## hasPassword
```js
true //비밀번호가 있을시에 true 반환
```
## validatePassword
```js
true //비밀번호가 맞을시에 true 반환
```
## selectUserGroup
```js
[
  {
    orgCode: '학교 코드',
    orgName: '학교 이름',
    userPNo: '고유 번호',
    userNameEncpt: '이름',
    stdntYn: 'Y',
    mngrYn: 'N',
    schulCrseScCode: '3', //학교 분류 코드(초등학교, 중학교, 고등학교 등)
    lctnScCode: '10', //학교 지역 분류 코드
    token: 'Bearer 토큰',
    atptOfcdcConctUrl: '관할 교육청 주소',
    wrongPassCnt: 0,
    otherYn: 'N' //다른 사람이 있는지
  }
]
```
## getUserInfo
```js
{
  orgCode: '학교 코드',
  orgName: '학교 이름',
  userPNo: '고유 번호',
  userNameEncpt: '학생 이름',
  userName: '학생 이름',
  stdntYn: 'Y',
  mngrClassYn: 'N',
  mngrDeptYn: 'N',
  schulCrseScCode: '3', //학교 분류 코드(초등학교, 중학교, 고등학교 등)
  lctnScCode: '10', //학교 지역 분류 코드
  insttClsfCode: '5',
  token: 'Bearer 토큰',
  atptOfcdcConctUrl: '관할 교육청 주소',
  registerDtm: '자가진단 한 시각',
  registerYmd: '자가진단 한 날짜',
  isHealthy: true, //자가진단 통과한지
  pInfAgrmYn: 'Y',
  admnYn: 'N',
  lockYn: 'N',
  wrongPassCnt: 0
}
```

# Api v1
1. WAFData - https://hcs.eduro.go.kr/
2. SearchSchool - https://hcs.eduro.go.kr/school
3. LoginToken - https://${atptOfcdcConctUrl}/loginwithschool
4. selectGroupList - https://${atptOfcdcConctUrl}/selectGroupList - Deprecated (v2 로 연결)
5. UserRefresh - https://${atptOfcdcConctUrl}/userrefresh
6. Servey - https://${atptOfcdcConctUrl}/registerServey

# Api v2
1. searchSchool - https://hcs.eduro.go.kr/v2/searchSchool
2. findUser - https://${atptOfcdcConctUrl}/v2/findUser
3. hasPassword - https://${atptOfcdcConctUrl}/v2/hasPassword
4. validatePassword - https://${atptOfcdcConctUrl}/v2/validatePassword
5. selectGroupList - https://${atptOfcdcConctUrl}/v2/selectUserGroup
6. getUserInfo - https://${atptOfcdcConctUrl}/v2/getUserInfo
7. registerServey - v1 의 registerServey 그대로 사용
