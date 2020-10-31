package Diagnosis;

public enum schulCrseScCodes {
    유치원(1),
    초등학교(2),
    중학교(3),
    고등학교(4),
    특수학교(5);

    final private int code;

    private schulCrseScCodes(int code) {
        this.code = code;
    }
    public int getCode() {
        return code;
    }
}
