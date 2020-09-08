# self-diagnosis
(교육부 자가진단)자가진단 하는 자바스크립트 함수
# 함수 설명
## SearchSchool
학교를 검색하고 결과를 반환합니다.
## LoginToken
학교의 orgCode 와 이름, 생년월일을 받고 인증용 토큰을 반환합니다.
## selectGroupList
토큰을 받고 유저 목록을 반환합니다.
## UserRefresh
토큰과 학교의 orgCode 그리고 userPNo 를 받아서 유저 정보를 반환합니다.
## Servey
토큰을 받고 자가진단 설문을 제출합니다.

# 예시
## 바로 자가진단
AutoCheck(lctnScCodes.경기도, schulCrseScCodes.중학교, "XXX중학교", "XXX", "200907");
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
