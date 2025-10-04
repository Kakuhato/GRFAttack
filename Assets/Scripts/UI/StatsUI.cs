using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI baseText;
    [SerializeField] private TextMeshProUGUI changeText;
    [SerializeField] private StatsType statsType = StatsType.Attack;

    private int lastValue;
    private int diffSum = 0;
    private Coroutine changeCoroutine;

    private void Awake()
    {
    }

    public void UpdateValue(PublishStats publishStats)
    {
        int newValue = statsType switch
        {
            StatsType.Attack => publishStats.Attack,
            StatsType.ShootRange => publishStats.ShootRange,
            StatsType.Speed => publishStats.Speed,
            StatsType.ShootSpeed => publishStats.ShootSpeed,
            _ => throw new ArgumentOutOfRangeException()
        };

        if (lastValue == 0)
        {
            lastValue = newValue;
            baseText.text = lastValue.ToString();
            return;
        }

        int diff = newValue - lastValue;
        if (diff == 0) return;

        diffSum += diff;
        baseText.text = newValue.ToString();
        lastValue = newValue;

        if (changeCoroutine == null)
            changeCoroutine = StartCoroutine(AniChange());
    }


    private IEnumerator AniChange()
    {
        bool isChanged = true;
        CountdownTimer timer = new CountdownTimer(1f);
        timer.OnTimerStop += () => { isChanged = false; };
        timer.Start();
        int lastDiff = 0;
        while (isChanged)
        {
            if (diffSum != lastDiff)
            {
                lastDiff = diffSum;
                changeText.text = lastDiff >= 0 ? $"+{lastDiff}" : lastDiff.ToString();
                changeText.color = lastDiff >= 0 ? Color.blue : Color.red;
                changeText.gameObject.SetActive(true);
                timer.Reset();
            }

            timer.Tick(Time.deltaTime);

            yield return null;
        }

        diffSum = 0;

        changeText.gameObject.SetActive(false);
        changeCoroutine = null;
    }
}