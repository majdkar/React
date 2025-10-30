using System.Collections.Generic;

namespace SchoolV01.Application.Features.Dashboards.Dtos;
public record DailyScheduleResponse(string Class, string Branch, List<PortionResponse> Portions);

public record PortionResponse(int PortionOrder, string ClassSubjectName, string InstructorName, string MeetingUrl);


