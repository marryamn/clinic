Feature: AppointmentPay
  exist some patient and some appointmentPay and some insurances
  Check NotComplete Installment and InsurancePaidAmount

    Scenario: GetNotCompleteInstallment work Successfully
        Given the following patient exist in the database
          | Id | Name   | Phone       | InsuranceId
          | 1  | maryam | 09142564968 | 1
          | 2  | User   | 09142589636 | 1
        And the following appointmentPay items  in database
          | Id | PatientId | IsPaid | PaidTime   | InsuranceId | Price | AppointmentId |
          | 1  | 1         | false  | 2023-04-05 | 1           | 1000  | 1             |
          | 2  | 1         | true   | 2023-04-01 | 1           | 2000  | 1             |
          | 3  | 1         | true   | 2023-04-01 | 2           | 2000  | 1             |
          | 4  | 1         | true   | 2023-04-01 | 1           | 4000  | 1             |
        When I Send Request to GetNotCompleteInstallment with this input
          | patientId | IsPaid | Page | pageSize
          | 1         | false  | 1    | 10
        Then the GetNotCompleteInstallment data is correct

    Scenario: GetNotCompleteInstallment Give Error
        When I Send Request to GetNotCompleteInstallment with this Not Correct input
          | patientId | IsPaid | Page | pageSize
          | 3         | false  | 1    | 10
        Then the GetNotCompleteInstallment data is not correct
        

        
    Scenario: GetInsurancePaidAppointmentPay work Successfully
        Given the following insurance exist in the database
          | Id | Name   |
          | 1  | Iran   |
          | 2  | Mellat |
      And the following appointmentPay items  in database again
        | Id | PatientId | IsPaid | PaidTime   | InsuranceId | Price | AppointmentId |
        | 1  | 1         | false  | 2023-04-05 | 1           | 1000  | 1             |
        | 2  | 1         | true   | 2023-04-01 | 1           | 2000  | 1             |
        | 3  | 1         | true   | 2023-04-01 | 2           | 2000  | 1             |
        | 4  | 1         | true   | 2023-04-01 | 1           | 4000  | 1             |
        When I Send Request to GetInsurancePaidAppointmentPay with this input
          | InsuranceId |
          | 1           |
        Then the GetInsurancePaidAppointmentPay data is correct

    Scenario: GetInsurancePaidAppointmentPay Give Error
      When I Send Request to GetInsurancePaidAppointmentPay with this not Correct input
        | patientId | IsPaid | Page | pageSize
        | 3         | false  | 1    | 10
      Then the GetInsurancePaidAppointmentPay data is not correct