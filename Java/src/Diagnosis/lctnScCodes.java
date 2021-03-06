package Diagnosis;

public enum lctnScCodes {
    서울특별시(1),
    부산광역시(2),
    대구광역시(3),
    인천광역시(4),
    광주광역시(5),
    대전광역시(6),
    울산광역시(7),
    세종특별자치시(8),
    경기도(10),
    강원도(11),
    충청북도(12),
    충청남도(13),
    전라북도(14),
    전라남도(15),
    경상북도(16),
    경상남도(17),
    제주특별자치도(18);

    private final int code;

    private lctnScCodes(int code) {
        this.code = code;
    }
    public int getCode() {
        return code;
    }
}