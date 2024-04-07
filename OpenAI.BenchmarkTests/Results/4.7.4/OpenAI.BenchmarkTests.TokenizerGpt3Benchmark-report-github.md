```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3374/23H2/2023Update/SunValley3)
11th Gen Intel Core i9-11980HK 2.60GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.202
  [Host]     : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method             | Mean     | Error   | StdDev   |
|------------------- |---------:|--------:|---------:|
| EncodeOriginal     | 465.6 μs | 9.20 μs | 11.96 μs |
| TokenCountOriginal | 364.1 μs | 7.23 μs | 10.37 μs |
