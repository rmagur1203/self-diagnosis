# self-diagnosis
(교육부 자가진단)자가진단 관련 라이브러리
# 함수 설명
## SearchSchool
학교를 검색하고 atptOfcdcConctUrl 에 호스트를 저장하고 결과를 반환합니다.
## LoginToken
학교의 orgCode 와 이름, 생년월일을 받고 인증용 토큰을 반환합니다.
## selectGroupList
토큰을 받고 유저 목록을 반환합니다.
## UserRefresh
토큰과 학교의 orgCode 그리고 userPNo 를 받아서 유저 정보를 반환합니다.
## Servey
토큰을 받고 자가진단 설문을 제출합니다.

# 함수 반환값
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

사용법 예시

# 예시
## 바로 자가진단
AutoCheck(lctnScCodes.경기도, schulCrseScCodes.중학교, "XXX중학교", "XXX", "생년월일(주민등록번호 앞 6자리)");
## 단계
1. SearchSchool
2. LoginToken
3. Servey

or

1. SearchSchool
2. LoginToken
3. selectGroupList
4. UserRefresh
5. Servey
